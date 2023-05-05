using Grpc.Core;
using Todo;
using Todo.Models;

namespace Todo.Services
{
    public class TodoService : Todo.TodoService.TodoServiceBase
    {
        private readonly ILogger<TodoService> _logger;
        private readonly DBContext _dbContext;
        public TodoService(ILogger<TodoService> logger, DBContext dBContext)
        {
            _logger = logger;
            _dbContext = dBContext;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == 2);

            if (user == null)
                user = new MiniUsers() { UserId = 2 };

            Tasks task = new Tasks()
            {
                CreatedDate = DateTime.UtcNow,
                Text = "Hellow",
                Owner = user,
            };

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            //var x = _dbContext.Tasks.First();

            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}