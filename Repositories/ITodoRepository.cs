
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoModel>> GetTodosAsync(); 

        Task<TodoModel?> GetTodoByIdAsync(int id);

        Task AddTodoAsync(TodoModel todo);

        Task UpdateTodoAsync(TodoModel todo);

        Task DeleteTodoAsync(TodoModel todo);
    }
}