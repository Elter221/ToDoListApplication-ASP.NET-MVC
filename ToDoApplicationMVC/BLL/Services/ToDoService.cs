using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess.Entities;
using ToDoApplicationMVC.Models;
using ToDoApplicationMVC.Services.Interfaces;

namespace ToDoApplicationMVC.Services;

public class ToDoService(TodoListDbContext context) : IToDoService
{
    //Add ct to all methods
    public async Task CreateNewToDoInList(ToDoModel model, int listId)
    {
        var toDo = new ToDo()
        {
            Name = model.Name,
            Description = model.Description,
            CreationDate = model.CreatedAt,
            Deadline = model.Deadline,
            Status = model.Status switch
            {
                "InProgress" => Status.InProgress,
                "Completed" => Status.Completed,
                _ => Status.Failed,
            },
            ToDoListId = listId,
            UserId = (await context.Users.FirstAsync()).Id,
        };

        await context.ToDos.AddAsync(toDo);
        await context.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var country = await context.ToDos.FindAsync(id);

        if (country is null)
        {
            return false;
        }

        context.ToDos.Remove(country!);

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteTagFromToDo(int tagId, int toDoId)
    {
        var tag = await context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);

        if (tag is null)
        {
            return false;
        }

        var toDo = context.ToDos.Include(todo => todo.Tags).SingleOrDefault(t => t.Id == toDoId);
        if (toDo is null)
        {
            return false;
        }

        toDo.Tags.Remove(tag);
        await context.SaveChangesAsync();


        return true;
    }

    //ToDoModel содержит listId
    public async Task<bool> EditToDo(ToDoModel toDo, int listId)
    {
        if (await this.IsToDoNameExists(toDo.Name, listId))
        {
            return false;
        }

        var toDoToFind = await context.ToDos
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(x => x.Id == toDo.Id);
        if (toDoToFind == null)
        {
            return false;
        }
        toDoToFind.Name = toDo.Name;
        toDoToFind.Description = toDo.Description;
        toDoToFind.CreationDate = toDo.CreatedAt;
        toDoToFind.Deadline = toDo.Deadline;
        toDoToFind.Status = toDo.Status switch
        {
            "InProgress" => Status.InProgress,
            "Completed" => Status.Completed,
            _ => Status.Failed,
        };
        if (toDo.TagsInput != string.Empty)
        {
            var tag = await this.FindOrAddTagInDB(toDo.TagsInput);
            if (!toDoToFind.Tags.Any(t => t.Id == tag.Id))
            {
                toDoToFind.Tags.Add(tag);

            }
        }

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<TagModel>> GetTags()
    {
        var data = await context.Tags.ToListAsync();

        if (data == null)
        {
            return null!;
        }

        var tagModels = data.Select(x => new TagModel
        {
            Id = x.Id,
            Name = x.TagName,
        });

        return tagModels;
    }

    public async Task<ToDoModel> GetToDoModelById(int id, int listId)
    {
        var toDo = await context.ToDos.FindAsync(id);
        if (toDo is null)
        {
            return null!;
        }
        var toDoModel = new ToDoModel
        {
            Id = toDo.Id,
            Name = toDo.Name,
            Description = toDo.Description,
            CreatedAt = toDo.CreationDate,
            Deadline = toDo.Deadline,
            Status = toDo.Status.ToString(),
            ToDoListId = listId,
        };

        return toDoModel;
    }

    public async Task<ToDoModel> GetToDoWithTags(int id)
    {
        var data = await context.ToDos
            .FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
        {
            return null!;
        }

        var tagModels = await context.ToDos
            .Where(todo => todo.Id == id)
            .SelectMany(todo => todo.Tags)
            .Select(tag => new TagModel
            {
                Id = tag.Id,
                Name = tag.TagName,
            }).ToListAsync();

        var toDoModel = new ToDoModel()
        {
            Name = data.Name,
            Description = data.Description,
            CreatedAt = data.CreationDate,
            Deadline = data.Deadline,
            Status = data.Status.ToString(),
            ToDoListId = data.ToDoListId,
            Tags = tagModels
        };

        return toDoModel;
    }

    public async Task<IEnumerable<ToDoModel>> GetToDosByTag(int tagId)
    {
        var tagToDo = context.Tags.Include(tag => tag.ToDos).Where(tag => tag.Id == tagId);

        var toDosModel = await context.ToDos
        .Where(todo => todo.Tags.Any(tag => tag.Id == tagId))
        .Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,
        }).ToListAsync();

        return toDosModel;
    }

    public async Task<IEnumerable<ToDoModel>> SearchByType(int userId, string? search, string? searchType)
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

        var data = await query.ToListAsync();

        var toDosModel = data.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,
        }).ToArray();

        return toDosModel;
    }

    public async Task<IEnumerable<ToDoModel>> SortByParams(int userId, string? sortBy, string? sortOrder)
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

        var data = await query.ToListAsync();

        var toDosModel = data.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,
        }).ToArray();

        return toDosModel;
    }

    private async Task<bool> IsToDoNameExists(string name, int listId)
        => await context.ToDos
        .AnyAsync(c => c.Name == name && c.ToDoListId == listId);

    private async Task<Tag> FindOrAddTagInDB(string name)
    {
        var tag = await context.Tags
                .Select(x => x)
                .FirstOrDefaultAsync(t => t.TagName == name);
        if (tag == null)
        {
            tag = new Tag { TagName = name };
            await context.Tags.AddAsync(tag);
            await context.SaveChangesAsync();
        }

        return tag;
    }
}
