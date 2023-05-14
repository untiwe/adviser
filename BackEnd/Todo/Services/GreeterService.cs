using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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

        public override Task<SpanMessageReply> AddNewTodo(AddNewTodoRequest request, ServerCallContext context)
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


            return Task.FromResult(new SpanMessageReply());
        }

        public override Task<GetListTodoReply> GetListTodoUser(GetListTodoTodoRequest request, ServerCallContext context)
        {
            var task = _dbContext.Tasks.Include(t => t.Owner).Where(t => t.Owner.UserId == request.Userid).ToList();
            var todoes = _mapper.Map<IEnumerable<TodoModel>>(task);
            var GetListTodoReply = new GetListTodoReply();
            GetListTodoReply.Todolist.AddRange(todoes);
            return Task.FromResult(GetListTodoReply);
        }

        public override Task<SpanMessageReply> UpdateTodo(UpdateTodoRequest request, ServerCallContext context)
        {
            var task = GetTask(request.Todoid);
            task.Text = request.Text;
            task.UpdatedTime = DateTime.UtcNow;
            _dbContext.SaveChanges();
            return Task.FromResult(new SpanMessageReply());
        }

        public override Task<SpanMessageReply> CompleteTodo(TodoIdRequest request, ServerCallContext context)
        {
            var task = GetTask(request.Id);
            task.CompleteTime = DateTime.UtcNow;
            _dbContext.SaveChanges();
            return Task.FromResult(new SpanMessageReply());
        }

        public override Task<SpanMessageReply> DeleteTodo(TodoIdRequest request, ServerCallContext context)
        { 
            var task = GetTask(request.Id);
            task.DeleteTime = DateTime.UtcNow;
            _dbContext.SaveChanges();
            return Task.FromResult(new SpanMessageReply());
        }

        private Tasks GetTask(int taskId)
        {
            var task = _dbContext.Tasks.SingleOrDefault(task => task.Id == taskId);
            if (task == null)
                throw new ArgumentNullException("��� ������ � ����� id");
            return task;
        }
    }
}