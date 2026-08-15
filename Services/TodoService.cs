using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Extensions;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        // todo一覧取得
        public async Task<IEnumerable<TodoResponse>> GetTodosAsync()
        {
            var todos = await _repository.GetTodosAsync();
            return todos.Select(todo => todo.ToResponse());
        }

        // 特定のtodo取得
        public async Task<TodoResponse?> GetTodoByIdAsync(int id)
        {
            var todo = await _repository.GetTodoByIdAsync(id);
            if (todo is null)
            {
                return null;
            }

            return todo.ToResponse();
        } 

        // todoの作成
        public async Task<TodoResponse> CreateTodoAsync(CreateTodoRequest request)
        {
            var todo = new TodoModel
            {
                Title = request.Title,
                IsCompleted = request.IsCompleted,
                CreatedAt = DateTime.Now
            };

            await _repository.AddTodoAsync(todo);

            return todo.ToResponse();
        }

        // todoの更新
        public async Task<TodoResponse?> UpdateTodoAsync(int id, UpdateTodoRequest request)
        {
            var todo = await _repository.GetTodoByIdAsync(id);

            if (todo is null)
            {
                return null;
            }

            todo.Title = request.Title;
            todo.IsCompleted = request.IsCompleted;

            await _repository.UpdateTodoAsync(todo);

            return todo.ToResponse();
        } 

        // todoを削除
        public async Task<bool> DeleteTodoAsync(int id)
        {
            var todo = await _repository.GetTodoByIdAsync(id);

            if(todo is null)
            {
                return false;
            }

            await _repository.DeleteTodoAsync(todo);

            return true;
        }
    }
}