using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
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

        public override Task<AddnewTodoReply> AddnewTodo(AddnewTodoRequest request, ServerCallContext context)
        {

            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == 2);

            if (user == null)
                user = new MiniUsers() { UserId = request.Userid};

            Tasks task = new Tasks()
            {
                CreatedDate = DateTime.UtcNow,
                Text = request.Text,
                Owner = user,
            };

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();


            return Task.FromResult(new AddnewTodoReply());
        }
    }
}