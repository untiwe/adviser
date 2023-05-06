using Microsoft.EntityFrameworkCore;
using Todo.AutoMapper;
using Todo.Models;
using Todo.Services;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001);
});

builder.Services.AddGrpc();

var ConnectionString = Environment.GetEnvironmentVariable("ConnectionString");
builder.Services.AddDbContext<DBContext>(options => options.UseNpgsql(ConnectionString));
builder.Services.AddAutoMapper(typeof(TodoModelProfile));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DBContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.MapGrpcService<TodoService>();

app.Run();
