using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace todo_api.Data;

/// <summary>
/// TodoアプリのDBコンテキスト
/// </summary>
public class TodoDbContext : DbContext
{
  /// <summary>
  /// コンストラクタ
  /// </summary>
  /// <param name="options"></param>
  public TodoDbContext(DbContextOptions<TodoDbContext> options)
          :base(options)
  {
  }

  /// <summary>
  /// Todoテーブル
  /// </summary>
  /// <value></value>
  public DbSet<TodoModel> Todos { get; set; } = null!;
  
}