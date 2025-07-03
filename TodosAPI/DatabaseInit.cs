using TodosAPI.Data;

namespace TodosAPI;

public static class DatabaseInit
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodosDbContext>();
        dbContext.Database.EnsureCreated();
    }
}