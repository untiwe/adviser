using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Todo;

namespace Main.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TodoController : ControllerBase
    {
        [HttpPost]
        public async  Task<IActionResult> Index(string name)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var client = new TodoService.TodoServiceClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
            return Ok(reply);
        }
    }
}
