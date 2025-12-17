using Microsoft.AspNetCore.Mvc;
using SIG_T.API.Services.Interfaces;
using SIG_T.Shared.Domain.DTOs;

namespace SIG_T.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TareasController : ControllerBase
{
    private readonly ITareaService _tareaService;

    public TareasController(ITareaService tareaService) => _tareaService = tareaService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _tareaService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var t = await _tareaService.GetByIdAsync(id);
        if (t == null) return NotFound();
        return Ok(t);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TareaCreateDTO dto)
    {
        var newId = await _tareaService.CreateAsync(dto);
        if (newId <= 0) return BadRequest(new { Message = "No se pudo crear la tarea" });
        return CreatedAtAction(nameof(GetById), new { id = newId }, new { Id = newId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TareaUpdateDTO dto)
    {
        var ok = await _tareaService.UpdateAsync(id, dto);
        if (!ok) return NotFound();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _tareaService.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}