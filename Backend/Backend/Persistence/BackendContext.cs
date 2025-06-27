using Microsoft.EntityFrameworkCore;
using Task = Backend.Domain.Task;

namespace Backend.Persistence;

public class BackendContext : DbContext
{
    private readonly IConfiguration config;

    public DbSet<Task> Tasks { get; set; }

    public BackendContext(IConfiguration config)
    {
        this.config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(config.GetConnectionString("BackendDatabase"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>().HasKey(x => x.Id);
    }
}