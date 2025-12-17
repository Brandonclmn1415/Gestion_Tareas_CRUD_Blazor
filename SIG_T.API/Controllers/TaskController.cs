using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIG_T.Shared.Models;
using SIG_T.API.Services.Implementations;
using SIG_T.API.Services.Interfaces;

namespace SIG_T.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskItem taskItem)
    {
        var result = await _taskService.CreateTaskAsync(taskItem);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _taskService.GetAllTasksAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _taskService.GetTaskByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskItem taskItem)
    {
        var result = await _taskService.UpdateTaskAsync(id, taskItem);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _taskService.DeleteTaskAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}
