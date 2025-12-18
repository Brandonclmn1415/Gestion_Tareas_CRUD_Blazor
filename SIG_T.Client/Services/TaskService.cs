using SIG_T.Shared.Models;
using SIG_T.Client.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;


namespace SIG_T.Client.Services
{
    public class TaskService : ITaskService
    {
        private readonly HttpClient _httpClient;

        public TaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private JsonSerializerOptions _jsonSerializerOptions => new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tasks", taskItem);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TaskItem>(_jsonSerializerOptions);
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<TaskItem>>("api/tasks", _jsonSerializerOptions);

            return result ?? Enumerable.Empty<TaskItem>();
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<TaskItem>($"api/tasks/{id}", _jsonSerializerOptions);
            return result;
        }

        public async Task<TaskItem> UpdateTaskAsync(int id, TaskItem taskItem)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/tasks/{id}", taskItem);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TaskItem>(_jsonSerializerOptions);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/tasks/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
