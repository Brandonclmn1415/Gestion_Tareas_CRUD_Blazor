using SIG_T.Shared.Models;

namespace SIG_T.API.Services.Interfaces;

public interface ITaskService
{
    Task<TaskItem> CreateTaskAsync(TaskItem taskItem);
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<TaskItem> GetTaskByIdAsync(int id);
    Task<TaskItem> UpdateTaskAsync(int id, TaskItem taskItem);
    Task<bool> DeleteTaskAsync(int id);
}
