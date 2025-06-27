using LeyfThings.Models;
using Microsoft.EntityFrameworkCore;

namespace LeyfThings.LeyfThingsDB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Goal> Goals { get; set; }
        public DbSet<SubGoals> SubGoals { get; set; }
    }

}
