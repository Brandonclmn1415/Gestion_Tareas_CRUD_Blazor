using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;
using SIG_T.Persistence.Data;
using SIG_T.Domain.DTO.Tarea;
using SIG_T.Application.Services.Implementations;

namespace SIG_T.Tests;

public class IntegrationTests
{
    private ApplicationDbContext CreateContext()
    {
        return TestDbContextFactory.Create();
    }

    [Fact]
    public async Task CreateTarea_UsesStoredProc_And_RegistroDeActividadLogged()
    {
        using var ctx = CreateContext();

        // Ensure there is at least one user for the task owner
        var user = await ctx.Usuarios.FirstOrDefaultAsync();
        if (user == null)
        {
            user = new Domain.Entities.Usuario { Nombre = "Test", Apellido = "User", Email = $"test{Guid.NewGuid()}@local.test", Activo = true };
            ctx.Usuarios.Add(user);
            await ctx.SaveChangesAsync();
        }

        var service = new TareaService(ctx);

        var dto = new TareaCreateDTO
        {
            Titulo = "Test Create",
            Descripcion = "Created by integration test",
            Estado = 0,
            FechaVencimiento = DateTime.UtcNow.AddDays(7),
            UsuarioId = user.Id
        };

        var newId = await service.CreateAsync(dto);
        newId.Should().BeGreaterThan(0);

        var logged = await ctx.RegistroDeActividad.AnyAsync(r => r.TareaId == newId && r.Accion == "CREATE");
        logged.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateTarea_UsesStoredProc_And_RegistroDeActividadLogged()
    {
        using var ctx = CreateContext();

        var user = await ctx.Usuarios.FirstOrDefaultAsync();
        if (user == null)
        {
            user = new Domain.Entities.Usuario { Nombre = "Test", Apellido = "User", Email = $"test{Guid.NewGuid()}@local.test", Activo = true };
            ctx.Usuarios.Add(user);
            await ctx.SaveChangesAsync();
        }

        var service = new TareaService(ctx);

        var dto = new TareaCreateDTO
        {
            Titulo = "Test Update",
            Descripcion = "Created by integration test",
            Estado = 0,
            FechaVencimiento = DateTime.UtcNow.AddDays(7),
            UsuarioId = user.Id
        };

        var newId = await service.CreateAsync(dto);
        newId.Should().BeGreaterThan(0);

        var updateDto = new TareaUpdateDTO
        {
            Id = newId,
            Titulo = "Test Update Changed",
            Descripcion = "Updated by integration test",
            Estado = 1,
            FechaVencimiento = DateTime.UtcNow.AddDays(10),
            UsuarioId = user.Id
        };

        var ok = await service.UpdateAsync(newId, updateDto);
        ok.Should().BeTrue();

        var loggedUpdate = await ctx.RegistroDeActividad.AnyAsync(r => r.TareaId == newId && r.Accion == "UPDATE");
        loggedUpdate.Should().BeTrue();
    }



    [Fact]
    public async Task ReportQueue_Enqueue_And_Dequeue_Works()
    {
        using var ctx = CreateContext();

        var newIdParam = new SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
        var enqueueResult = await ctx.ExecuteStoredProcNonQueryAsync("sp_ReportRequest_Enqueue",
            new SqlParameter("@ReportType", System.Data.SqlDbType.NVarChar, 100) { Value = "TareasFinalizadas" },
            new SqlParameter("@Payload", System.Data.SqlDbType.NVarChar, -1) { Value = DBNull.Value },
            newIdParam);

        // Dequeue next pending
        var idOut = new SqlParameter("@Id", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
        await ctx.ExecuteStoredProcNonQueryAsync("sp_ReportRequest_Dequeue", idOut);

        var dequeued = idOut.Value switch { int i => i, decimal d => (int)d, _ => 0 };
        dequeued.Should().BeGreaterThan(0);

        // Mark processed
        var idParam = new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = dequeued };
        await ctx.ExecuteStoredProcNonQueryAsync("sp_ReportRequest_MarkProcessed", idParam);
    }
}