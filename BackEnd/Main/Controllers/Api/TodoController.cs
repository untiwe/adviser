using Grpc.Net.Client;
using Main.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo;

namespace Main.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TodoController : ControllerBase

    {
        private readonly string _todoUrl;
        private readonly DBContext _dbContext;
        public TodoController(DBContext dbContext)
        {
            _dbContext = dbContext;

            _todoUrl = Environment.GetEnvironmentVariable("TodoServiseURL");
            if (string.IsNullOrEmpty(_todoUrl))
            {
                throw new ArgumentException("Нет URL для подключения к сервису");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddnewTodo(string text)
        {
            var userid = GetUserId();
            using var channel = GrpcChannel.ForAddress(_todoUrl);
            var client = new TodoService.TodoServiceClient(channel);
            var reply = await client.AddnewTodoAsync(new AddnewTodoRequest { Userid = userid, Text = text});
            return Ok(reply);
        }

        private int GetUserId()
        {
            FormattableString requrstSQL = $"SELECT \"Users\".\"Id\" from public.\"Users\" WHERE \"Login\" = {HttpContext.User.Identity.Name}";
            return _dbContext.Database.SqlQuery<int>(requrstSQL).ToList().First();
        }
    }
}
