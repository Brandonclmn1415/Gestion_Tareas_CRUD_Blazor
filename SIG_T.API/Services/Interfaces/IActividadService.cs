using SIG_T.Shared.Domain.DTOs;

namespace SIG_T.API.Services.Interfaces;

/// <summary>
/// Interface for activity logging service
/// </summary>
public interface IActividadService
{
    /// <summary>
    /// Gets all activity log entries
    /// </summary>
    /// <returns>List of activity log entries</returns>
    Task<List<RegistroActividadDto>> GetAllActividadesAsync();
}