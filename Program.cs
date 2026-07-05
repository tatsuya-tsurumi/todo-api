using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var todos = new List<TodoModel>
{
    new()
    {
        Id = 1,
        Title = "ASP.NET Coreを勉強する",
        IsCompleted = false
    },
    new()
    {
        Id = 2,
        Title = "Swaggerを確認する",
        IsCompleted = true
    }
};

/// <summary>
/// Todo一覧を取得するAPI
/// </summary>
app.MapGet("/todos", () => todos)
    .WithName("GetTodos")
    .WithSummary("Todo一覧を取得する")
    .WithDescription("登録されているTodo一覧を返却します。");

// <summary>
/// 指定したTodoを取得するAPI
/// </summary>
app.MapGet("/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);

    return todo is null ? Results.NotFound() : Results.Ok(todo);
})
    .WithName("GetTodoById")
    .WithSummary("指定したIDのTodoを取得する")
    .WithDescription("IDに一致するTodoを返却します。存在しない場合は404 Not Foundを返却します。");

// <summary>
/// Todoを作成するAPI
/// </summary>
app.MapPost("/todos", (TodoModel todo) =>
{
    // todoのIDを生成
    var newId = todos.Any() ? todos.Max(t => t.Id) + 1 : 1;

    // todoに必要な情報を追記
    todo.Id = newId;
    todo.CreatedAt = DateTime.UtcNow;

    todos.Add(todo);

    return Results.Created($"/todos/{todo.Id}", todo);
})
    .WithName("CreateTodo")
    .WithSummary("Todoを新規作成する")
    .WithDescription("新しいTodoを登録し、作成したTodoを返却します。");

// <summary>
/// Todoを更新するAPI
/// </summary>
app.MapPut("/todos/{id}", (int id, TodoModel request) => 
{
    var todo = todos.FirstOrDefault(t => t.Id == id);

    if (todo is null)
    {
        return Results.NotFound();
    }

    todo.Title = request.Title;
    todo.IsCompleted = request.IsCompleted;

    return Results.Ok(todo);
})
    .WithName("UpdateTodo")
    .WithSummary("指定したIDのTodoを更新する")
    .WithDescription("IDに一致するTodoを更新します。存在しない場合は404 Not Foundを返却します。");

app.Run();