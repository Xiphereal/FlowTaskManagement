using Backend.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddSingleton<BackendContext>();
builder.Services.AddSingleton<TestingDatabaseInitializer>();
builder.Services.AddSingleton<ITaskRepository, EntityFrameworkTaskRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.Services.GetRequiredService<TestingDatabaseInitializer>().Execute();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();