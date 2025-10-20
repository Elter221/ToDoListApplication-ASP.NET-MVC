using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess.Entities;
using ToDoApplicationMVC.DataAccess.Interfaces;

namespace ToDoApplicationMVC.DataAccess;

public class ToDoRepository(TodoListDbContext context) : IToDoRepository
{
    public async Task<int> Create(ToDo model, CancellationToken cancellationToken = default)
    {
        if (await context.ToDos
        .AnyAsync(c => c.Name == model.Name && c.ToDoListId == model.ToDoListId, cancellationToken))
        {
            return -1;
        }

        //Иницилизировать полупустой ТуДу в вызывающем коде и передать сюда
        //Status = model.Status switch
        //{
        //    "InProgress" => Status.InProgress,
        //    "Completed" => Status.Completed,
        //    _ => Status.Failed,
        //},

        model.UserId = (await context.Users.FirstOrDefaultAsync(cancellationToken))!.Id;

        await context.ToDos.AddAsync(model, cancellationToken);
        return (await context.ToDos.LastOrDefaultAsync(cancellationToken))!.Id;
    }

    public async Task Delete(int id, CancellationToken cancellationToken = default)
    {
        var todo = await context.ToDos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (todo != null)
        {
            context.ToDos.Remove(todo);
        }
    }


    public async Task<bool> Update(ToDo model, CancellationToken cancellationToken = default)
    {
        if (await context.ToDos
        .AnyAsync(c => c.Name == model.Name && c.ToDoListId == model.ToDoListId, cancellationToken))
        {
            return false;
        }

        var toDoToFind = await context.ToDos
            .FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
        if (toDoToFind != null)
        {
            toDoToFind.Name = model.Name;
            toDoToFind.Description = model.Description;
            toDoToFind.CreationDate = model.CreationDate;
            toDoToFind.Deadline = model.Deadline;
            toDoToFind.Status = model.Status;
            return true;
        }
        return false;
    }

    public async Task AddTagToDo(int toDoId, string tagName, CancellationToken cancellationToken = default)
    {
        var toDoToFind = await context.ToDos
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(x => x.Id == toDoId, cancellationToken);

        if (tagName != string.Empty && toDoToFind != null)
        {
            var tag = await this.FindOrAddTagInDB(tagName);
            if (!toDoToFind.Tags.Any(t => t.Id == tag.Id))
            {
                toDoToFind.Tags.Add(tag);
            }
        }
    }

    public IQueryable<ToDo> GetAll()
        => context.ToDos.Select(x => x);

    public async Task<ToDo?> GetById(int id, CancellationToken cancellationToken = default)
        => await context.ToDos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    //Task?
    public IQueryable<ToDo> GetToDosByTag(int tagId) =>
        context.ToDos
        .Where(todo => todo.Tags.Any(tag => tag.Id == tagId))
        .Select(x => x);

    public async Task<ToDo?> GetToDoWithTags(int id, CancellationToken cancellationToken = default) =>
        await context.ToDos
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? null;

    public IQueryable<ToDo> SearchByType(int userId, string? search, string? searchType)
    {
        bool isValidDate = DateOnly.TryParseExact(search, "dd.MM.yyyy", out DateOnly date);

        var query = context.ToDos
            .Where(param => param.UserId == userId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = searchType switch
            {
                "name" => query.Where(c => c.Name.Contains(search)),
                "status" => query.Where(c => c.Status.ToString() == search),
                "deadline" => isValidDate ? query.Where(c => c.Deadline <= date) : query.Where(c => false),
                _ => query,
            };
        }

        return query;
    }

    public IQueryable<ToDo> SortByParams(int userId, string? sortBy, string? sortOrder)
    {
        var query = context.ToDos
            .Where(param => param.UserId == userId);

        query = (sortBy?.ToLower(), sortOrder?.ToLower()) switch
        {

            ("name", "asc") => query.OrderBy(t => t.Name),
            ("name", "desc") => query.OrderByDescending(t => t.Name),

            ("status", "asc") => query.OrderBy(t => t.Status),
            ("status", "desc") => query.OrderByDescending(t => t.Status),

            ("deadline", "asc") => query.OrderBy(t => t.Deadline),
            ("deadline", "desc") => query.OrderByDescending(t => t.Deadline),

            (_, "asc") => query.OrderBy(t => t.CreationDate),
            (_, "desc") => query.OrderByDescending(t => t.CreationDate),

            _ => query
        };

        return query;
    }

    private async Task<Tag> FindOrAddTagInDB(string name, CancellationToken cancellationToken = default)
    {
        var tag = await context.Tags
                .Select(x => x)
                .FirstOrDefaultAsync(t => t.TagName == name, cancellationToken);
        if (tag == null)
        {
            tag = new Tag { TagName = name };
            await context.Tags.AddAsync(tag, cancellationToken);
        }

        return tag;
    }
}
