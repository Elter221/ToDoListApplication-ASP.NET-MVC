using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.DAL;

public class ToDoListRepository(TodoListDbContext context) : Repository<ToDoList>(context), IToDoListRepository
{
    public new async Task<int> Create(ToDoList model, CancellationToken cancellationToken = default)
    {
        if (await this.DbSet.AnyAsync(c => c.Name == model.Name, cancellationToken))
        {
            return -1;
        }

        //var toDoList = new ToDoList()
        //{
        //    Name = model.Name,
        //    CreationDate = model.CreatedAt,
        //    NumberOfTasks = 0,
        //};
        return await base.Create(model, cancellationToken);
    }

    public IQueryable<ToDo> GetToDosOfList(int listId) => this.DbSet.Where(x => x.Id == listId).SelectMany(x => x.ToDos).AsQueryable();

    public new async Task<bool> Update(ToDoList model, CancellationToken cancellationToken = default)
    {
        if (await this.DbSet.AnyAsync(c => c.Name == model.Name, cancellationToken))
        {
            return false;
        }

        return await base.Update(model, cancellationToken);
    }
}
