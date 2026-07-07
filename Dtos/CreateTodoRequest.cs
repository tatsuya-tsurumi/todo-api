using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos;

public class CreateTodoRequest
{
  /// <summary>
  /// Todoのタイトル
  /// </summary>
  /// <value></value>
  [Required]
  [StringLength(100)]
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// Todoの完了状態
  /// </summary> <summary>
  /// 
  /// </summary>
  /// <value></value>
  public bool IsCompleted { get; set; }
}