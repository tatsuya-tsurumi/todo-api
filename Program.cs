using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using todo_api.Data;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/// <summary>
/// Todo一覧を取得するAPI
/// </summary>
app.MapGet("/todos", async (TodoDbContext db) =>
{
    return await db.Todos.ToListAsync();
})
    .WithName("GetTodos")
    .WithSummary("Todo一覧を取得する")
    .WithDescription("登録されているTodo一覧を返却します。");

// <summary>
/// 指定したTodoを取得するAPI
/// </summary>
app.MapGet("/todos/{id}", async (int id, TodoDbContext db) =>
{
    var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == id);

    return todo is null ? Results.NotFound() : Results.Ok(todo);
})
    .WithName("GetTodoById")
    .WithSummary("指定したIDのTodoを取得する")
    .WithDescription("IDに一致するTodoを返却します。存在しない場合は404 Not Foundを返却します。");

// <summary>
/// Todoを作成するAPI
/// </summary>
app.MapPost("/todos", async (TodoModel todo, TodoDbContext db) =>
{
    todo.CreatedAt = DateTime.UtcNow;

    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todos/{todo.Id}", todo);
})
    .WithName("CreateTodo")
    .WithSummary("Todoを新規作成する")
    .WithDescription("新しいTodoを登録し、作成したTodoを返却します。");

// <summary>
/// Todoを更新するAPI
/// </summary>
app.MapPut("/todos/{id}", async (int id, TodoModel request, TodoDbContext db) => 
{
    var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == id);

    if (todo is null)
    {
        return Results.NotFound();
    }

    todo.Title = request.Title;
    todo.IsCompleted = request.IsCompleted;

    await db.SaveChangesAsync();

    return Results.Ok(todo);
})
    .WithName("UpdateTodo")
    .WithSummary("指定したIDのTodoを更新する")
    .WithDescription("IDに一致するTodoを更新します。存在しない場合は404 Not Foundを返却します。");

// <summary>
/// Todoを削除するAPI
/// </summary>
app.MapDelete("/todos/{id}", async (int Id, TodoDbContext db) =>
{
    var todo = await db.Todos.FirstOrDefaultAsync(t => t.Id == Id);

    if (todo is null)
    {
        return Results.NotFound();
    }

    db.Todos.Remove(todo);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
    .WithName("DeleteTodo")
    .WithSummary("指定したIDのTodoを削除する")
    .WithDescription("IDに一致するTodoを削除します。存在しない場合は404 Not Foundを返却します。");



app.Run();