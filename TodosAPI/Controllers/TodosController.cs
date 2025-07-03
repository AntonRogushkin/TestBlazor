using Microsoft.AspNetCore.Mvc;
using TodosAPI.Data;
using Dto;

namespace TodosAPI.Controllers;

[Route("api/[controller]")]
public class TodosController(TodosDbContext todosDbContext) : ApplicationControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<TodoDto>> GetTodos()
    {
        var todos = todosDbContext.Todos.AsEnumerable().Select(todo => new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            DueDate = todo.DueDate,
            IsComplete = todo.IsComplete,
            CreatedAt = todo.CreatedAt,
            UpdatedAt = todo.UpdatedAt
        }).ToList();
        
        return Ok(todos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoDto>> GetTodo(int id)
    {
        var todoDto = await todosDbContext.Todos.FindAsync(id);

        if (todoDto == null)
        {
            return NotFound();
        }

        return Ok(todoDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodo([FromBody] TodoDto? todoDto)
    {
        if (todoDto == null)
        {
            return BadRequest();
        }

        var newTodo = new Todo
        {
            Title = todoDto.Title,
            Description = todoDto.Description,
            DueDate = DateTime.Now.Add(TimeSpan.FromDays(2)),
            IsComplete = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        try
        {
            todosDbContext.Todos.Add(newTodo);
            await todosDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateTodo), new { Id = newTodo.Id }, newTodo);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var toDelete = await todosDbContext.Todos.FindAsync(id);

        if (toDelete == null)
        {
            return NotFound();
        }

        todosDbContext.Todos.Remove(toDelete);

        await todosDbContext.SaveChangesAsync();
        
        return NoContent();
    }
}