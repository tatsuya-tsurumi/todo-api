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

app.MapGet("/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);

    return todo is null ? Results.NotFound() : Results.Ok(todo);
})
    .WithName("GetTodoById")
    .WithSummary("指定したIDのTodoを取得する")
    .WithDescription("IDに一致するTodoを返却します。存在しない場合は404 Not Foundを返却します。");

app.Run();