

using TodoApi.Dtos;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoResponse>> GetTodosAsync();
        Task<TodoResponse?> GetTodoByIdAsync(int id);
        Task<TodoResponse> CreateTodoAsync(CreateTodoRequest request);
        Task<TodoResponse?> UpdateTodoAsync(int id, UpdateTodoRequest request);

        Task<bool> DeleteTodoAsync(int id);
    }
}