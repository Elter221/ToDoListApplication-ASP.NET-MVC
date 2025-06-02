
using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess;

namespace ToDoApplicationMVC.Services;

public class DbInitService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        _ = await dbContext.Database.EnsureCreatedAsync(stoppingToken);

        if (!await dbContext.Users.AnyAsync(stoppingToken))
        {
            _ = dbContext.Users.Add(new User
            {
                Name = "Admin",
                Email = "admin@gmail.com",
                Password = "12345",
            });
        }

        _ = await dbContext.SaveChangesAsync(stoppingToken);

        if (!await dbContext.ToDoLists.AnyAsync(stoppingToken))
        {
            dbContext.ToDoLists.AddRange([
                new ToDoList{
                    Name = "To-Do 1",
                    CreationDate = new DateOnly(2025, 5, 13),
                    NumberOfTasks = 1,
                },
                new ToDoList{
                    Name = "To-Do 2",
                    CreationDate = new DateOnly(2025, 3, 13),
                    NumberOfTasks = 2,
                }
                ]);
        }

        _ = await dbContext.SaveChangesAsync(stoppingToken);

        if (!await dbContext.ToDos.AnyAsync(stoppingToken))
        {
            dbContext.ToDos.AddRange([
                    new ToDo
                    {
                        Name = "Houseworks",
                        Description = "Wash dishes",
                        CreationDate = new DateOnly(2025, 5, 13),
                        Deadline = new DateOnly(2025, 5, 14),
                        Status = Status.InProgress,
                        ToDoListId = (await dbContext.ToDoLists.FirstAsync()).Id,
                        UserId = (await dbContext.ToDoLists.FirstAsync()).Id
                    },
                    new ToDo
                    {
                        Name = "Study ASP",
                        Description = "Write MVC 1 Part",
                        CreationDate = new DateOnly(2025, 3, 13),
                        Deadline = new DateOnly(2025, 5, 14),
                        Status = Status.Completed,
                        ToDoListId = (await dbContext.ToDoLists.FirstAsync()).Id,
                        UserId = (await dbContext.ToDoLists.FirstAsync()).Id
                    },
                    new ToDo
                    {
                        Name = "Study Patterns",
                        Description = "Write Patterns HW2",
                        CreationDate = new DateOnly(2025, 4, 13),
                        Deadline = new DateOnly(2025, 5, 13),
                        Status = Status.Failed,
                        ToDoListId = (await dbContext.ToDoLists.FirstAsync()).Id,
                        UserId = (await dbContext.ToDoLists.FirstAsync()).Id
                    }
                ]);
        }

        _ = await dbContext.SaveChangesAsync(stoppingToken);
    }
}
