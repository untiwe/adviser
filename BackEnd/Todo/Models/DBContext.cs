using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Todo.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions opt) :
        base(opt)
        { }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<MiniUsers> Users { get; set; }
    }
}
