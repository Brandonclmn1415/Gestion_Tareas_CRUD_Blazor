using Gestion_Tareas.Shared.Models;
using Gestion_Tareas_API.Data;
using Gestion_Tareas_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Gestion_Tareas_API.Services.Implementaciones
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem)
        {
            var entity = new TaskItem
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Status = taskItem.Status,
                DueDate = taskItem.DueDate
            };

            _context.TaskItems.Add(entity);
            await _context.SaveChangesAsync();

            taskItem = new TaskItem
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Status = entity.Status,
                DueDate = entity.DueDate
            };

            return taskItem;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            var list = await _context.TaskItems.ToListAsync();

            return list.Select(t => new TaskItem 
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                DueDate = t.DueDate
            });
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id) 
        {
            var t = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id);

            if (t == null) 
                return null;
            
            return new TaskItem 
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                DueDate = t.DueDate
            };
        }

        public async Task<TaskItem> UpdateTaskAsync(int id, TaskItem taskItem)
        {
            var entity = await _context.TaskItems.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return null;

            entity.Id = taskItem.Id;
            entity.Title = taskItem.Title;
            entity.Description = taskItem.Description;
            entity.Status = taskItem.Status;
            entity.DueDate = taskItem.DueDate;

            await _context.SaveChangesAsync();

            return taskItem;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var entity = await _context.TaskItems.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            _context.TaskItems.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
