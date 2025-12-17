using Microsoft.Extensions.Configuration;
using SIG_T.API.Data;
using Microsoft.EntityFrameworkCore;

namespace SIG_T.Worker;

/// <summary>
/// Background service for generating task reports
/// </summary>
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ApplicationDbContext _context;

    public Worker(ILogger<Worker> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Try to process all queued report requests
                while (await TryProcessNextReportAsync())
                {
                    // keep processing until queue is empty
                }

                // If nothing to process, wait a bit before polling again
                await Task.Delay(30000, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during report generation");
                
                await Task.Delay(60000, stoppingToken);
            }
        }
    }

    /// <summary>
    /// Gets the count of completed tasks
    /// </summary>
    /// <returns>Number of completed tasks</returns>
    private async Task<int> GetCompletedTasksCountAsync()
    {
        return await _context.Tareas.CountAsync(t => t.Estado == 2);
    }

    /// <summary>
    /// Generates a report of completed tasks using EF Core (no raw SQL)
    /// </summary>
    private async Task GenerateCompletedTasksReportAsync(int? requestId = null)
    {
        var completed = await _context.Tareas
            .Where(t => t.Estado == 2)
            .OrderByDescending(t => t.FechaCompletada)
            .Select(t => new {
                t.Id,
                t.Titulo,
                t.Descripcion,
                FechaCompletada = t.FechaCompletada,
                UsuarioCompleto = t.Usuario.Nombre + " " + t.Usuario.Apellido
            })
            .ToListAsync();

        var reportLines = new List<string>();
        reportLines.Add("=== REPORTE DE TAREAS COMPLETADAS ===");
        reportLines.Add($"Fecha de generación: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        if (requestId.HasValue)
            reportLines.Add($"RequestId: {requestId.Value}");
        reportLines.Add("");

        foreach (var row in completed)
        {
            reportLines.Add($"ID: {row.Id}");
            reportLines.Add($"Título: {row.Titulo}");
            if (!string.IsNullOrWhiteSpace(row.Descripcion))
                reportLines.Add($"Descripción: {row.Descripcion}");
            if (row.FechaCompletada.HasValue)
                reportLines.Add($"Completada el: {row.FechaCompletada:yyyy-MM-dd HH:mm:ss}");
            reportLines.Add($"Usuario: {row.UsuarioCompleto}");
            reportLines.Add("---");
        }

        // Log the report content
        foreach (var line in reportLines)
        {
            _logger.LogInformation(line);
        }

        _logger.LogInformation("Report generation completed successfully.");
    }

    private async Task<bool> TryProcessNextReportAsync()
    {
        // Call the dequeue stored procedure to get the next pending request id
        var idOut = new Microsoft.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };

        await _context.ExecuteStoredProcNonQueryAsync("sp_ReportRequest_Dequeue", idOut);

        var nextId = idOut.Value switch
        {
            int i => i,
            decimal d => (int)d,
            _ => 0
        };

        if (nextId <= 0)
            return false; // nothing to process

        _logger.LogInformation("Processing report request {Id}", nextId);

        try
        {
            await GenerateCompletedTasksReportAsync(nextId);

            var idParam = new Microsoft.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = nextId };
            await _context.ExecuteStoredProcNonQueryAsync("sp_ReportRequest_MarkProcessed", idParam);

            _logger.LogInformation("Report request {Id} processed", nextId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process report request {Id}", nextId);
            // In a robust system we'd mark it failed or retry; for now we leave it as Processing to be retried later
            return false;
        }
    }
}
