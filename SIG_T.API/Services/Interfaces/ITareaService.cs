using SIG_T.Shared.Domain.DTOs;

namespace SIG_T.API.Services.Interfaces;

public interface ITareaService
{
    Task<IEnumerable<TareaResponseDTO>> GetAllAsync();
    Task<TareaResponseDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(TareaCreateDTO dto);
    Task<bool> UpdateAsync(int id, TareaUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}