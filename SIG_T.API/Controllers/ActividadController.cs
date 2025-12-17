using Microsoft.AspNetCore.Mvc;
using SIG_T.Shared.Domain.DTOs;
using SIG_T.API.Services.Interfaces;

namespace SIG_T.API.Controllers
{
    /// <summary>
    /// Controller for activity logging endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadController : ControllerBase
    {
        private readonly IActividadService _actividadService;

        public ActividadController(IActividadService actividadService)
        {
            _actividadService = actividadService;
        }

        /// <summary>
        /// Gets all activity log entries
        /// </summary>
        /// <returns>List of activity log entries</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _actividadService.GetAllActividadesAsync();
            return Ok(result);
        }
    }
}