using Microsoft.EntityFrameworkCore;
using TaskManager.models;

namespace TaskManager.dbcontext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
}