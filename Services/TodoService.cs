using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Extensions;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly TodoDbContext _context;

        public TodoService(TodoDbContext context)
        {
            _context = context;
        }

        // todo一覧取得
        public async Task<IEnumerable<TodoResponse>> GetTodosAsync()
        {
            return await _context.Todos
                .Select(todo => todo.ToResponse())
                .ToListAsync();
        }

        // 特定のtodo取得
        public async Task<TodoResponse?> GetTodoByIdAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
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
                CreatedAt = DateTime.UtcNow
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return todo.ToResponse();
        }

        // todoの更新
        public async Task<TodoResponse?> UpdateTodoAsync(int id, UpdateTodoRequest request)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo is null)
            {
                return null;
            }

            todo.Title = request.Title;
            todo.IsCompleted = request.IsCompleted;

            await _context.SaveChangesAsync();

            return todo.ToResponse();
        } 

        // todoを削除
        public async Task<bool> DeleteTodoAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo is null)
            {
                return false;
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return true;

        }
    }
}