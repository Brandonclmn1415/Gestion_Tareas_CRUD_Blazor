using SIG_T.Domain.DTO.Tarea;

namespace SIG_T.Application.Services.Interfaces;

public interface ITaskService
{
    Task<TareaResponseDTO> CreateTaskAsync(TareaCreateDTO taskItem);
    Task<IEnumerable<TareaResponseDTO>> GetAllTasksAsync();
    Task<TareaResponseDTO?> GetTaskByIdAsync(int id);
    Task<TareaResponseDTO?> UpdateTaskAsync(int id, TareaUpdateDTO taskItem);
    Task<bool> DeleteTaskAsync(int id);
}
