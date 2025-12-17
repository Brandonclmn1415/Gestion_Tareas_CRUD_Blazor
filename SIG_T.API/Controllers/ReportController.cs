using Microsoft.AspNetCore.Mvc;
using SIG_T.Shared.Domain.DTOs;

namespace SIG_T.API.Controllers
{
    /// <summary>
    /// Controller for reporting endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        /// <summary>
        /// Endpoint for reporting completed tasks
        /// Returns HTTP 202 Accepted to indicate the request was accepted for processing
        /// </summary>
        /// <returns>Accepted result</returns>
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context) => _context = context;

        [HttpPost("tareas-finalizadas")]
        public async Task<IActionResult> ReportCompletedTasks()
        {
            // Enqueue a report request in the database using stored procedure
            var newIdParam = new Microsoft.Data.SqlClient.SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
            var reportTypeParam = new Microsoft.Data.SqlClient.SqlParameter("@ReportType", System.Data.SqlDbType.NVarChar, 100) { Value = "TareasFinalizadas" };
            var payloadParam = new Microsoft.Data.SqlClient.SqlParameter("@Payload", System.Data.SqlDbType.NVarChar, -1) { Value = DBNull.Value };

            await _context.ExecuteStoredProcNonQueryAsync("sp_ReportRequest_Enqueue", reportTypeParam, payloadParam, newIdParam);

            var newId = (newIdParam.Value is int nid) ? nid : (newIdParam.Value is decimal dec ? (int)dec : 0);

            return Accepted(new ApiResponseDTO<object>
            {
                Success = true,
                Message = "Solicitud de reporte de tareas finalizadas aceptada para procesamiento.",
                Data = new { RequestId = newId }
            });
        }
    }
}