using Microsoft.EntityFrameworkCore;
using TodosAPI;
using TodosAPI.Data;

const string allowBlazorClientOrigin = "BlazorClientOrigin";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowBlazorClientOrigin, policy =>
    {
        policy.WithOrigins("http://localhost:5219");
    });
});
builder.Services.AddControllers();
builder.Services.AddDbContext<TodosDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(allowBlazorClientOrigin);
app.UseAuthorization();

app.MapControllers();

try
{
    DatabaseInit.Initialize(app.Services);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();