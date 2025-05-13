using Microsoft.EntityFrameworkCore;

namespace ToDoApplicationMVC.DataAccess;

public class TodoListDbContext : DbContext
{
    public DbSet<ToDo> ToDos { get; set; }
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }
}
