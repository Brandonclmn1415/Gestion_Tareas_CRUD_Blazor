using SIG_T.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using SIG_T.Domain.DTO.Tarea;
using SIG_T.Domain.Entities;
using SIG_T.Application.Services.Interfaces;

namespace SIG_T.Application.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TareaResponseDTO> CreateTaskAsync(TareaCreateDTO taskItem)
    {
        var entity = new Tarea
        {
            Id =  Convert.ToInt32(new Guid().ToString()),
            Titulo = taskItem.Titulo,
            Descripcion = taskItem.Descripcion,
            Estado = taskItem.Estado,
            FechaVencimiento = taskItem.FechaVencimiento
        };

        _context.Tareas.Add(entity);
        await _context.SaveChangesAsync();

        var entityResponse = new TareaResponseDTO
        {
            Id = entity.Id,
            Titulo = entity.Titulo,
            Descripcion = entity.Descripcion,
            Estado = entity.Estado,
            FechaVencimiento = entity.FechaVencimiento
        };

        return entityResponse;
    }

    public async Task<IEnumerable<TareaResponseDTO>> GetAllTasksAsync()
    {
        var list = await _context.Tareas.ToListAsync();

        return list.Select(t => new TareaResponseDTO 
        {
            Id = t.Id,
            Titulo = t.Titulo,
            Descripcion = t.Descripcion,
            Estado = t.Estado,
            FechaVencimiento = t.FechaVencimiento
        });
    }

    public async Task<TareaResponseDTO?> GetTaskByIdAsync(int id)
    {
        var t = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id);

        if (t == null) 
            return null;
        
        return new TareaResponseDTO
        {
            Id = t.Id,
            Titulo = t.Titulo,
            Descripcion = t.Descripcion,
            Estado = t.Estado,
            FechaVencimiento = t.FechaVencimiento
        };
    }

    public async Task<TareaResponseDTO?> UpdateTaskAsync(int id, TareaUpdateDTO taskItem)
    {
        var entity = await _context.Tareas.FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
            return null;

        entity.Id = taskItem.Id;
        entity.Titulo = taskItem.Titulo;
        entity.Descripcion = taskItem.Descripcion;
        entity.Estado = taskItem.Estado;
        entity.FechaVencimiento = taskItem.FechaVencimiento;

        await _context.SaveChangesAsync();

        var taskUpdated = new TareaResponseDTO
        {
            Id = entity.Id,
            Titulo = entity.Titulo,
            Descripcion = entity.Descripcion,
            Estado = entity.Estado,
            FechaVencimiento = entity.FechaVencimiento
        };

        return taskUpdated;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var entity = await _context.Tareas.FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
            return false;

        _context.Tareas.Remove(entity);

        await _context.SaveChangesAsync();

        return true;
    }
}
