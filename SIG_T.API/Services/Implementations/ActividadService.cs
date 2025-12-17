using Microsoft.Data.SqlClient;
using SIG_T.API.Data;
using SIG_T.API.Services.Interfaces;
using SIG_T.Shared.Domain.DTOs;
using SIG_T.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace SIG_T.API.Services.Implementations
{
    public class ActividadService : IActividadService
    {
        private readonly ApplicationDbContext _context;

        public ActividadService(ApplicationDbContext context) => _context = context;

        /// <summary>
        /// Gets all activity log entries using Entity Framework
        /// </summary>
        /// <returns>List of activity log entries</returns>
        public async Task<List<RegistroActividadDto>> GetAllActividadesAsync()
        {
            var resultado = await _context.QueryStoredProcAsync("sp_RegistroActividad_Listar", reader => new RegistroActividadDto
            {
                Id = reader.GetInt32(0),
                TareaId = reader.GetInt32(1),
                Accion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                FechaRegistro = reader.GetDateTime(3),
                Usuario = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
            });

            return resultado;
        }
    }
}