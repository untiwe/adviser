namespace Todo.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set;}
        public DateTime? DeleteTime { get; set; }
        public MiniUsers Owner { get; set; }

    }
}
