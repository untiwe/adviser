namespace Todo.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set;}
        public DateTime DeleteTime { get; set; }
        public DateTime CompleteTime { get; set; }
        public MiniUsers Owner { get; set; }

    }
}
