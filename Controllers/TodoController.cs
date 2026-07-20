using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Extensions;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
  private readonly ITodoService _todoService;

  public TodoController(ITodoService todoService)
  {
    _todoService = todoService;
  }

  /// <summary>
  /// Todo一覧を取得
  /// </summary>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<TodoResponse>>> GetTodos()
  {
    var todos = await _todoService.GetTodosAsync();

    return Ok(todos);
  }

  /// <summary>
  /// 指定したTodoを取得
  /// </summary>
  [HttpGet("{id}")]
  public async Task<ActionResult<TodoResponse>> GetTodo(int id)
  {
    var todo = await _todoService.GetTodoByIdAsync(id);

    if (todo is null)
    {
      return NotFound();
    }

    return Ok(todo);
  }

  // <summary>
  /// Todoを作成するAPI
  /// </summary>
  [HttpPost]
  public async Task<ActionResult<TodoResponse>> CreateTodo([FromBody] CreateTodoRequest request)
  {
    var todo = _todoService.CreateTodoAsync(request);

    return CreatedAtAction(
      nameof(GetTodo), 
      new {id = todo.Id}, 
      todo
    );
  }

  // <summary>
  /// Todoを更新
  /// </summary>
  [HttpPut("{id}")]
  public async Task<ActionResult<TodoResponse>> UpdateTodo(
    int id,
    [FromBody] UpdateTodoRequest request)
  {
    var todo = await _todoService.UpdateTodoAsync(id, request);

    if (todo is null)
    {
      return NotFound();
    }

    return Ok(todo);
  }

  // <summary>
  /// Todoを削除
  /// </summary>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteTodo(int id)
  {
    var result = await _todoService.DeleteTodoAsync(id);

    if (!result)
    {
      return NotFound();
    }

    return NoContent();
  }

}