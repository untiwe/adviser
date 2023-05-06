using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Todo;
using Todo.AutoMapper;
using Todo.Models;

namespace Todo.Services
{
    public class TodoService : Todo.TodoService.TodoServiceBase
    {
        private readonly ILogger<TodoService> _logger;
        private readonly DBContext _dbContext;
        private readonly IMapper _mapper;
        public TodoService(ILogger<TodoService> logger, DBContext dBContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dBContext;
            _mapper = mapper;
        }

        public override Task<AddNewTodoReply> AddNewTodo(AddNewTodoRequest request, ServerCallContext context)
        {

            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == 1);

            if (user == null)
                user = new MiniUsers() { UserId = request.Userid };

            Tasks task = new Tasks()
            {
                CreatedTime = DateTime.UtcNow,
                Text = request.Text,
                Owner = user,
            };

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();


            return Task.FromResult(new AddNewTodoReply());
        }

        public override Task<GetListTodoReply> GetListTodoUser(GetListTodoTodoRequest request, ServerCallContext context)
        {
            var task = _dbContext.Tasks.Include(t => t.Owner).Where(t => t.Owner.UserId == request.Userid).ToList();
            var todoes = _mapper.Map<IEnumerable<TodoModel>>(task);
            var GetListTodoReply = new GetListTodoReply();
            GetListTodoReply.Todolist.AddRange(todoes);
            return Task.FromResult(GetListTodoReply);
        }
    }
}