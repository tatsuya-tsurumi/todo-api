using Moq;
using TodoApi.Services;
using TodoApi.Repositories;
using TodoApi.Models;
using TodoApi.Dtos;

public class TodoServiceTests
{
  // 全件取得
  [Fact]
  public async Task GetTodosAsync_Returnstodos()
  {
    // Mockを作成
    var mockRepository = new Mock<ITodoRepository>();
    
    // Repositoryが返すデータの作成
    var expectedTodos = new List<TodoModel>
    {
      new TodoModel
      {
        Id = 1,
        Title = "テスト",
        IsCompleted = false,
        CreatedAt = DateTime.Now
      }
    };

    // Mockの動きを設定
    mockRepository
      .Setup(r => r.GetTodosAsync())
      .ReturnsAsync(expectedTodos);

    var service = new TodoService(mockRepository.Object);

    // Act
    var result = await service.GetTodosAsync();

    // Assert
    Assert.Single(result);

    var todo = result.Single();

    Assert.Equal("テスト", todo.Title);
    Assert.False(todo.IsCompleted);
  }

  // IDを指定してtodoを取得
  [Fact]
  public async Task GetTodoByIdAsync_ReturnsTodo_WhenExists()
  {
    // Mockを作成
    var mockRepository = new Mock<ITodoRepository>();
    
    // Repositoryが返すデータの作成
    var expectedTodo = new TodoModel
    {
        Id = 1,
        Title = "テスト",
        IsCompleted = false,
        CreatedAt = DateTime.Now
    };

    // Mockの動きを設定
    mockRepository
      .Setup(r => r.GetTodoByIdAsync(1))
      .ReturnsAsync(expectedTodo);

    var service = new TodoService(mockRepository.Object);

    // Act
    var result = await service.GetTodoByIdAsync(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("テスト", result.Title);
    Assert.False(result.IsCompleted);
  }

  // IDを指定したがtodoが存在しなかった場合
  [Fact]
  public async Task GetTodoByIdAsync_ReturnsNull_WhenNotFound()
  {
    // Mockを作成
    var mockRepository = new Mock<ITodoRepository>();

    // Mockの動きを設定
    mockRepository
      .Setup(r => r.GetTodoByIdAsync(1))
      .ReturnsAsync((TodoModel?)null);

    var service = new TodoService(mockRepository.Object);

    // Act
    var result = await service.GetTodoByIdAsync(1);

    // Assert
    Assert.Null(result);
  }

  // todoの作成
  [Fact]
  public async Task CreateTodoAsync_ReturnsCreatedTodo()
  {
    var mockRepository = new Mock<ITodoRepository>();

    var request = new CreateTodoRequest
    {
      Title = "テスト",
      IsCompleted = false
    };

    var service = new TodoService(mockRepository.Object);

    var result = await service.CreateTodoAsync(request);

    Assert.Equal("テスト", result.Title);
    Assert.False(result.IsCompleted);

    mockRepository.Verify(
      r => r.AddTodoAsync(
        It.Is<TodoModel>(todo =>
          todo.Title == "テスト" &&
          todo.IsCompleted == false
        )),
      Times.Once);
  }

  // 対象todoの更新
  [Fact]
  public async Task UpdateTodoAsync_ReturnsUpratedTodo()
  {
    // Arrange
    var mockRepository = new Mock<ITodoRepository>();

    var existTodo = new TodoModel
    {
      Id = 1,
      Title = "更新前",
      IsCompleted = false,
      CreatedAt = DateTime.Now
    };

    var request = new UpdateTodoRequest
    {
      Title = "更新後",
      IsCompleted = true
    };

    mockRepository
      .Setup(r => r.GetTodoByIdAsync(1))
      .ReturnsAsync(existTodo);

    // Act
    var service = new TodoService(mockRepository.Object);
    var result = await service.UpdateTodoAsync(1, request);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("更新後", result.Title);
    Assert.True(result.IsCompleted);
    mockRepository.Verify(
      r => r.UpdateTodoAsync(
        It.Is<TodoModel>(todo =>
          todo.Title == "更新後" &&
          todo.IsCompleted == true
        )),
      Times.Once
    );
  }

  // 対象todoが存在しない場合
  [Fact]
  public async Task UpdateTodoAsync_ReturnsNull_WhenNotFound()
  {
    // Arrange
    // Mockを作成
    var mockRepository = new Mock<ITodoRepository>();

    var request = new UpdateTodoRequest
    {
      Title = "更新後",
      IsCompleted = true
    };

    // Mockの動きを設定
    mockRepository
      .Setup(r => r.GetTodoByIdAsync(1))
      .ReturnsAsync((TodoModel?)null);

    var service = new TodoService(mockRepository.Object);

    // Act
    var result = await service.UpdateTodoAsync(1, request);

    // Assert
    Assert.Null(result);
    mockRepository.Verify(
      r => r.UpdateTodoAsync(It.IsAny<TodoModel>()),
      Times.Never
    );
  }

  // 対象todoを削除
  [Fact]
  public async Task DeleteTodoAsync_ReturnsTrue_WhenTodoExists()
  {
    // Arrange
    var mockRepository = new Mock<ITodoRepository>();

    var existTodo = new TodoModel
    {
      Id = 1,
      Title = "削除対象のtodo",
      IsCompleted = false,
      CreatedAt = DateTime.Now
    };

    mockRepository
      .Setup(r => r.GetTodoByIdAsync(1))
      .ReturnsAsync(existTodo);

    // Act
    var service = new TodoService(mockRepository.Object);
    var result = await service.DeleteTodoAsync(1);

    // Assert
    Assert.True(result);
    mockRepository.Verify(
      r => r.DeleteTodoAsync(existTodo),
      Times.Once
    );
  }

  // 削除対象が存在しない場合
  [Fact]
  public async Task DeleteTodoAsync_ReturnsFalse_WhenTodoNotFound()
  {
    // Arrange
    var mockRepository = new Mock<ITodoRepository>();

    mockRepository
      .Setup(r => r.GetTodoByIdAsync(1))
      .ReturnsAsync((TodoModel?)null);

    // Act
    var service = new TodoService(mockRepository.Object);
    var result = await service.DeleteTodoAsync(1);

    // Assert
    Assert.False(result);
    mockRepository.Verify(
      r => r.DeleteTodoAsync(It.IsAny<TodoModel>()),
      Times.Never
    );  
  }
}
