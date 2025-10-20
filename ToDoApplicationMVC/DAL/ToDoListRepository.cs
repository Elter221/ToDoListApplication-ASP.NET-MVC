using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess.Entities;
using ToDoApplicationMVC.DataAccess.Interfaces;

namespace ToDoApplicationMVC.DataAccess;

public class ToDoListRepository(TodoListDbContext context) : IToDoListRepository
{
    public async Task<int> Create(ToDoList model, CancellationToken cancellationToken = default)
    {
        if (await context.ToDoLists.AnyAsync(c => c.Name == model.Name, cancellationToken))
        {
            return -1;
        }

        //var toDoList = new ToDoList()
        //{
        //    Name = model.Name,
        //    CreationDate = model.CreatedAt,
        //    NumberOfTasks = 0,
        //};

        await context.ToDoLists.AddAsync(model, cancellationToken);

        return (await context.ToDoLists.LastOrDefaultAsync(cancellationToken))!.Id;
    }

    public async Task Delete(int id, CancellationToken cancellationToken = default)
    {
        var list = await context.ToDoLists.FindAsync(id, cancellationToken);
        if (list != null)
        {
            context.Remove(list);
        }
    }

    public IQueryable<ToDoList> GetAll() => context.ToDoLists.Select(x => x);

    public async Task<ToDoList?> GetById(int id, CancellationToken cancellationToken = default)
        => await context.ToDoLists.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public IQueryable<ToDo> GetToDosOfList(int listId) => context.ToDos.Where(todo => todo.Id == listId);

    public async Task<bool> Update(ToDoList model, CancellationToken cancellationToken = default)
    {
        if (await context.ToDoLists.AnyAsync(c => c.Name == model.Name, cancellationToken))
        {
            return false;
        }

        var listToFind = await context.ToDoLists.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

        if (listToFind != null)
        {
            listToFind.Name = model.Name;
            listToFind.CreationDate = model.CreationDate;
        }

        return true;
    }
}
