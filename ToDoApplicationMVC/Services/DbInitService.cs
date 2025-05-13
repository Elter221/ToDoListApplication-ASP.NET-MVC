
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

        if (!await dbContext.ToDos.AnyAsync(stoppingToken))
        {
            dbContext.ToDos.AddRange([
                    new ToDo
                    {
                        Name = "ToDo-1",
                        Description = "Wash dishes",
                        CreationDate = new DateOnly(2025, 5, 13),
                        Deadline = new DateOnly(2025, 5, 14),
                        Status = Status.InProgress,
                    },
                    new ToDo
                    {
                        Name = "ToDo-2",
                        Description = "Write MVC 1 Part",
                        CreationDate = new DateOnly(2025, 3, 13),
                        Deadline = new DateOnly(2025, 5, 14),
                        Status = Status.Completed,
                    },
                    new ToDo
                    {
                        Name = "ToDo-3",
                        Description = "Write Patterns HW2",
                        CreationDate = new DateOnly(2025, 4, 13),
                        Deadline = new DateOnly(2025, 5, 13),
                        Status = Status.Failed,
                    }
                ]);

            _ = await dbContext.SaveChangesAsync(stoppingToken);
        }

    }
}
