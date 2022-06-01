using Microsoft.EntityFrameworkCore;
using XevosWebApp.Model;

namespace XevosWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Person>? People { get; set; }
    }
}
