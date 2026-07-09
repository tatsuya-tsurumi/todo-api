using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Extensions;
using TodoApi.Helpers;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
  private readonly TodoDbContext _context;

  public TodoController(TodoDbContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Todo一覧を取得
  /// </summary>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<TodoResponse>>> GetTodos()
  {
    var todos = await _context.Todos.ToListAsync();
    var response = todos.Select(todo => todo.ToResponse());

    return Ok(response);
  }

  /// <summary>
  /// 指定したTodoを取得
  /// </summary>
  [HttpGet("{id}")]
  public async Task<ActionResult<TodoResponse>> GetTodo(int id)
  {
    var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id);

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
    var todo = new TodoModel
    {
      Title = request.Title,
      IsCompleted = request.IsCompleted,
      CreatedAt = DateTime.UtcNow
    };

    _context.Todos.Add(todo);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetTodo), new {id = todo.Id}, todo.ToResponse());
  }

  // <summary>
  /// Todoを更新
  /// </summary>
  [HttpPut("{id}")]
  public async Task<ActionResult<TodoResponse>> UpdateTodo(
    int id,
    [FromBody] UpdateTodoRequest request)
  {
    var todo = await _context.Todos.FindAsync(id);

    if (todo is null)
    {
      return NotFound();
    }

    todo.Title = request.Title;
    todo.IsCompleted = request.IsCompleted;
    await _context.SaveChangesAsync();

    return Ok(todo.ToResponse());
  }

  // <summary>
  /// Todoを削除
  /// </summary>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeletTodo(int id)
  {
    var todo = await _context.Todos.FindAsync(id);

    if (todo is null)
    {
      return NotFound();
    }

    _context.Todos.Remove(todo);
    await _context.SaveChangesAsync();

    return NoContent();
  }

}