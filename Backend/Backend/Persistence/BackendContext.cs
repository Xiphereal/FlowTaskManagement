using Microsoft.EntityFrameworkCore;
using Task = Backend.Domain.Task;

namespace Backend.Persistence;

public class BackendContext : DbContext
{
    public DbSet<Task> Tasks { get; set; }

    private string DbPath { get; }

    public BackendContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "backend.db");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>().HasKey(x => x.Name);
    }
}