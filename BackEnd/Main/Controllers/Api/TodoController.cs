using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Todo;

namespace Main.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TodoController : ControllerBase

    {
        private readonly string _todoUrl;
        public TodoController()
        {
            _todoUrl = Environment.GetEnvironmentVariable("TodoServiseURL");
            if (string.IsNullOrEmpty(_todoUrl))
            {
                throw new ArgumentException("Нет URL для подключения к сервису");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Index(string name)
        {
            using var channel = GrpcChannel.ForAddress(_todoUrl);
            var client = new TodoService.TodoServiceClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
            return Ok(reply);
        }
    }
}
