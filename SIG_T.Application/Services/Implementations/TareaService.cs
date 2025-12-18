using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIG_T.Persistence.Data;
using SIG_T.Domain.DTO.Tarea;
using SIG_T.Application.Services.Interfaces;

namespace SIG_T.Application.Services.Implementations;

public class TareaService : ITareaService
{
    private readonly ApplicationDbContext _context;

    public TareaService(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<TareaResponseDTO>> GetAllAsync()
    {
        var list = await _context.Tareas
            .Include(t => t.Usuario)
            .OrderByDescending(t => t.FechaCreacion)
            .Select(t => new TareaResponseDTO
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descripcion = t.Descripcion,
                Estado = t.Estado,
                EstadoDescripcion = t.EstadoDescripcion,
                FechaCreacion = t.FechaCreacion,
                FechaVencimiento = t.FechaVencimiento,
                FechaCompletada = t.FechaCompletada,
                UsuarioId = t.UsuarioId,
                NombreUsuario = t.Usuario.Nombre + " " + t.Usuario.Apellido,
                EmailUsuario = t.Usuario.Email,
                EstaVencida = t.EstaVencida,
                DiasTranscurridos = t.DiasTranscurridos
            })
            .ToListAsync();

        return list;
    }

    public async Task<TareaResponseDTO?> GetByIdAsync(int id)
    {
        var t = await _context.Tareas.Include(t => t.Usuario).FirstOrDefaultAsync(t => t.Id == id);
        if (t == null) return null;
        return new TareaResponseDTO
        {
            Id = t.Id,
            Titulo = t.Titulo,
            Descripcion = t.Descripcion,
            Estado = t.Estado,
            EstadoDescripcion = t.EstadoDescripcion,
            FechaCreacion = t.FechaCreacion,
            FechaVencimiento = t.FechaVencimiento,
            FechaCompletada = t.FechaCompletada,
            UsuarioId = t.UsuarioId,
            NombreUsuario = t.Usuario.Nombre + " " + t.Usuario.Apellido,
            EmailUsuario = t.Usuario.Email,
            EstaVencida = t.EstaVencida,
            DiasTranscurridos = t.DiasTranscurridos
        };
    }

    public async Task<int> CreateAsync(TareaCreateDTO dto)
    {
        // Build parameters
        var titulo = new SqlParameter("@Titulo", SqlDbType.NVarChar, 200) { Value = dto.Titulo };
        var descripcion = new SqlParameter("@Descripcion", SqlDbType.NVarChar, 1000) { Value = (object?)dto.Descripcion ?? DBNull.Value };
        var estado = new SqlParameter("@Estado", SqlDbType.Int) { Value = dto.Estado };
        var fechaVenc = new SqlParameter("@FechaVencimiento", SqlDbType.DateTime2) { Value = (object?)dto.FechaVencimiento ?? DBNull.Value };
        var usuarioId = new SqlParameter("@UsuarioId", SqlDbType.Int) { Value = dto.UsuarioId };

        // Get user name
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == dto.UsuarioId);
        var usuarioNombre = usuario != null ? usuario.Nombre + " " + usuario.Apellido : string.Empty;
        var usuarioParam = new SqlParameter("@Usuario", SqlDbType.NVarChar, 100) { Value = usuarioNombre };

        var newIdParam = new SqlParameter("@NuevoId", SqlDbType.Int) { Direction = ParameterDirection.Output };

        await _context.ExecuteStoredProcNonQueryAsync("sp_Tareas_Create", titulo, descripcion, estado, fechaVenc, usuarioId, usuarioParam, newIdParam);

        return newIdParam.Value is int id ? id : 0;
    }

    public async Task<bool> UpdateAsync(int id, TareaUpdateDTO dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == dto.UsuarioId);
        var usuarioNombre = usuario != null ? usuario.Nombre + " " + usuario.Apellido : string.Empty;

        var parameters = new SqlParameter[]
        {
            new("@Id", SqlDbType.Int) { Value = id },
            new("@Titulo", SqlDbType.NVarChar, 200) { Value = dto.Titulo },
            new("@Descripcion", SqlDbType.NVarChar, 1000) { Value = (object?)dto.Descripcion ?? DBNull.Value },
            new("@Estado", SqlDbType.Int) { Value = dto.Estado },
            new("@FechaVencimiento", SqlDbType.DateTime2) { Value = (object?)dto.FechaVencimiento ?? DBNull.Value },
            new("@UsuarioId", SqlDbType.Int) { Value = dto.UsuarioId },
            new("@Usuario", SqlDbType.NVarChar, 100) { Value = usuarioNombre }
        };

        await _context.ExecuteStoredProcNonQueryAsync("sp_Tareas_Update", parameters);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var param = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
        await _context.ExecuteStoredProcNonQueryAsync("sp_Tareas_Delete", param);
        return true;
    }
}