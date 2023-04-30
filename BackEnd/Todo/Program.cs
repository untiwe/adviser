using Todo.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001);
});

builder.Services.AddGrpc();

var app = builder.Build();

app.UseStaticFiles();
app.MapGrpcService<TodoService>();
app.MapGet("", () => "Hello");

app.Run();
