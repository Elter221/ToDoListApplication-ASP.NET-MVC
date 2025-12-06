
using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DAL;
using ToDoApplicationMVC.DAL.Entities;

namespace ToDoApplicationMVC.BLL.Services;

public class DbInitService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        _ = await dbContext.Database.EnsureCreatedAsync(stoppingToken);

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

            var toDoListId = (await dbContext.ToDoLists.FirstAsync()).Id;
            var userId = await dbContext.Users
                                            .Where(x => x.Email == "elter@gmail.com")
                                            .Select(x => x.Id)
                                            .FirstOrDefaultAsync();
            dbContext.ToDos.AddRange([
                    new ToDo
                    {
                        Name = "Houseworks",
                        Description = "Wash dishes",
                        CreationDate = new DateOnly(2025, 5, 13),
                        Deadline = new DateOnly(2025, 5, 14),
                        Status = Status.InProgress,
                        ToDoListId = toDoListId,
                        UserId = userId,
                    },
                    new ToDo
                    {
                        Name = "Study ASP",
                        Description = "Write MVC 1 Part",
                        CreationDate = new DateOnly(2025, 3, 13),
                        Deadline = new DateOnly(2025, 5, 14),
                        Status = Status.Completed,
                        ToDoListId = toDoListId,
                        UserId = userId,
                    },
                    new ToDo
                    {
                        Name = "Study Patterns",
                        Description = "Write Patterns HW2",
                        CreationDate = new DateOnly(2025, 4, 13),
                        Deadline = new DateOnly(2025, 5, 13),
                        Status = Status.Failed,
                        ToDoListId = toDoListId,
                        UserId = userId,
                    }
                ]);
        }

        _ = await dbContext.SaveChangesAsync(stoppingToken);
    }
}
