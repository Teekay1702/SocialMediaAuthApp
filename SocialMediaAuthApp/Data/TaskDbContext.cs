using Microsoft.EntityFrameworkCore;
using SocialMediaAuthApp.Models;

namespace SocialMediaAuthApp.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options)
        {
        }
        public DbSet<TaskModel> Tasks { get; set; }
    }
}
