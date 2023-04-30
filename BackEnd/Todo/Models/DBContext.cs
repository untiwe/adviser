using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Todo.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions opt) :
        base(opt)
        { }

        public DbSet<Tasks> Users { get; set; }
    }
}
