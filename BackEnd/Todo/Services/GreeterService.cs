using Grpc.Core;
using Todo;

namespace Todo.Services
{
    public class TodoService : Todo.TodoService.TodoServiceBase
    {
        private readonly ILogger<TodoService> _logger;
        public TodoService(ILogger<TodoService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}