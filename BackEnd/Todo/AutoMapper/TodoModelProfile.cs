using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using System.Threading.Tasks;
using Todo.Models;

namespace Todo.AutoMapper
{

    public class TodoModelProfile : Profile
    {
        public TodoModelProfile()
        {
            CreateMap<Tasks, TodoModel>()
                .ForMember(model => model.CreatedTime, tasks => tasks.MapFrom(tasks => Format(tasks.CreatedTime)))
                .ForMember(model => model.UpdatedTime, tasks => tasks.MapFrom(tasks => Format(tasks.UpdatedTime)))
                .ForMember(model => model.CompleteTime, tasks => tasks.MapFrom(tasks => Format(tasks.СompleteTime)))
                .ForMember(model => model.DeleteTime, tasks => tasks.MapFrom(tasks => Format(tasks.DeleteTime)));
                

        }

        private string Format(DateTime date) => date.ToUniversalTime()
                         .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
    }

    public class TestModel
    {
        public int Id { get; set; }
        public Timestamp CreatedTime { get; set; }
    }
}
