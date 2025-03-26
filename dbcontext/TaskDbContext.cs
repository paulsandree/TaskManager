using Microsoft.EntityFrameworkCore;
using TaskManager.models;

namespace TaskManager.dbcontext;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }
    public DbSet<TaskItem> Tasks { get; set; }
}