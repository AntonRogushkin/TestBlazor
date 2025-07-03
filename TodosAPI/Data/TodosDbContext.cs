using Microsoft.EntityFrameworkCore;

namespace TodosAPI.Data;

public class TodosDbContext(DbContextOptions<TodosDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}