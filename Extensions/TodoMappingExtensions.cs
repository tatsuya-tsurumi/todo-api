using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Extensions;

public static class TodoMappingExtensions
{
  public static TodoResponse ToResponse(this TodoModel todo)
  {
    return new TodoResponse
    {
        Id = todo.Id,
        Title = todo.Title,
        IsCompleted = todo.IsCompleted,
        CreatedAt = todo.CreatedAt
    };

  }
}