using Microsoft.EntityFrameworkCore;

namespace Main.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions opt) :
            base(opt)
        { }

        public DbSet<User> Users { get; set; }
    }
}
