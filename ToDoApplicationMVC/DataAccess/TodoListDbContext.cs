using Microsoft.EntityFrameworkCore;

namespace ToDoApplicationMVC.DataAccess;

public class TodoListDbContext : DbContext
{
    public DbSet<ToDo> ToDos { get; set; }

    public DbSet<ToDoList> ToDoLists { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<TagToDo> TagToDos { get; set; }
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
