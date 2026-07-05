namespace TodoApi.Models;

public class TodoModel
{
  /// <summary>todoを識別するID</summary>
  public int Id { get; set; }

  /// <summary>タイトル</summary>
  public string Title { get; set; } = string.Empty;

  /// <summary>todoが完了しているかどうか</summary>
  public bool IsCompleted { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}