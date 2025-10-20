using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.BLL.Models;
using ToDoApplicationMVC.BLL.Services.Interfaces;
using ToDoApplicationMVC.DataAccess.Entities;

namespace ToDoApplicationMVC.BLL.Services;

public class ToDoListService(TodoListDbContext context) : IToDoListService
{
    public async Task<bool> AddNewToDoList(ToDoListModel model)
    {
        if (await this.ListNameExists(model.Name))
        {
            return false;
        }

        var toDoList = new ToDoList()
        {
            Name = model.Name,
            CreationDate = model.CreatedAt,
            NumberOfTasks = 0,
        };

        _ = context.ToDoLists.Add(toDoList);
        _ = await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteToDoList(int id)
    {
        var todoList = await context.ToDoLists
        .Include(t => t.ToDos)
        .SingleOrDefaultAsync(t => t.Id == id);

        if (todoList == null)
        {
            return false;
        }

        context.ToDos.RemoveRange(todoList.ToDos);

        _ = context.ToDoLists.Remove(todoList);

        _ = await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EditToDoList(ToDoListModel model)
    {
        if (await this.ListNameExists(model.Name))
        {
            return false;
        }

        var listToFind = await context.ToDoLists.FirstAsync(x => x.Id == model.Id);

        listToFind.Name = model.Name;
        listToFind.CreationDate = model.CreatedAt;

        _ = await context.SaveChangesAsync();

        return true;
    }
    // Cancelation Token
    public async Task<IReadOnlyList<ToDoListModel>> GetToDoLists()
    {
        var data = context.ToDoLists.Include(x => x.ToDos);

        var toDosModel = data.Select(x => new ToDoListModel()
        {
            Id = x.Id,
            Name = x.Name,
            CreatedAt = x.CreationDate,
            NumberOfTasks = x.ToDos.Count,
        });

        return await toDosModel.ToListAsync();
    }

    public async Task<ToDoListModel> GetToDoList(int id)
    {
        var toDoList = await context.ToDoLists.FindAsync(id);
        if (toDoList == null)
        {
            return null!;
        }

        var toDoModel = new ToDoListModel
        {
            Id = toDoList.Id,
            Name = toDoList.Name,
            CreatedAt = toDoList.CreationDate,
            NumberOfTasks = toDoList.NumberOfTasks,
        };

        return toDoModel;
    }

    public async Task<IEnumerable<ToDoModel>> GetToDosOfList(int listId)
    {
        var data = await context.ToDoLists
            .Include(x => x.ToDos)
            .SingleOrDefaultAsync(x => x.Id == listId);

        if (data == null)
        {
            return null!;
        }

        var toDosModel = data.ToDos.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = listId,
        }).ToArray();

        return toDosModel;
    }

    private async Task<bool> ListNameExists(string name)
        => await context.ToDoLists.AnyAsync(c => c.Name == name);
}
