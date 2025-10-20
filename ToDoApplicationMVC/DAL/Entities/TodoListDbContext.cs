using Microsoft.EntityFrameworkCore;

namespace ToDoApplicationMVC.DataAccess.Entities;

public class TodoListDbContext : DbContext
{
    public DbSet<ToDo> ToDos { get; set; }

    public DbSet<ToDoList> ToDoLists { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDo>(entity =>
        {
            entity.HasKey(todo => todo.Id);
            entity.Property(todo => todo.Description).HasMaxLength(50);
            entity.HasOne(todo => todo.User).WithMany(user => user.ToDos).HasForeignKey(todo => todo.UserId);
            entity.HasMany(todo => todo.Tags).WithMany(tag => tag.ToDos);
            entity.HasOne(todo => todo.ToDoList).WithMany(todoList => todoList.ToDos).HasForeignKey(todo => todo.ToDoListId);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(comm => comm.Id);
            entity.Property(comm => comm.Description).HasMaxLength(100);
            entity.HasOne(comm => comm.ToDo).WithMany(todo => todo.Comments).HasForeignKey(comm => comm.ToDoId);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(tag => tag.Id);
        });

        modelBuilder.Entity<ToDoList>(entity =>
        {
            entity.HasKey(list => list.Id);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);
        });
        base.OnModelCreating(modelBuilder);
    }
}
