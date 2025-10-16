using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess;
using ToDoApplicationMVC.Models;
using ToDoApplicationMVC.Services.Interfaces;

namespace ToDoApplicationMVC.Services;

public class ToDoService(TodoListDbContext context) : IToDoService
{
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

        _ = context.ToDos.Add(toDo);
        _ = await context.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var country = await context.ToDos.FindAsync(id);

        if (country is null)
        {
            return false;
        }

        _ = context.ToDos.Remove(country!);

        _ = await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteTag(int tagId, int toDoId)
    {
        var tag = await context.Tags.FindAsync(tagId);

        if (tag is null)
        {
            return false;
        }

        context.TagToDos
            .RemoveRange(
                await context.TagToDos
                .Select(x => x)
                .Where(x => x.TagsId == tagId && x.ToDoId == toDoId)
                .ToArrayAsync());

        _ = await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EditToDo(ToDoModel toDo, int listId)
    {
        if (await this.IsToDoNameExists(toDo.Name, listId))
        {
            return false;
        }

        var toDoToFind = await context.ToDos.FirstAsync(x => x.Id == toDo.Id);

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
            var tagId = await this.FindOrAddTagInDB(toDo.TagsInput);
            if (!context.TagToDos.Any(x => x.TagsId == tagId && x.ToDoId == toDoToFind.Id))
            {
                _ = await context.TagToDos!.AddAsync(
                new TagToDo
                {
                    TagsId = tagId,
                    ToDoId = toDoToFind.Id
                });
            }
        }

        _ = await context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<TagModel>> GetTags()
    {
        var data = await context.Tags
            .ToListAsync();

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

        var tags = await context.TagToDos
                .Where(x => x.ToDoId == data.Id)
                .Join(context.Tags,
                    tagToDo => tagToDo.TagsId,
                    tag => tag.Id,
                    (tagToDo, tag) => tag)
                .Select(x => new TagModel()
                {
                    Id = x.Id,
                    Name = x.TagName
                })
                .ToListAsync();

        var toDoModel = new ToDoModel()
        {
            Name = data.Name,
            Description = data.Description,
            CreatedAt = data.CreationDate,
            Deadline = data.Deadline,
            Status = data.Status.ToString(),
            ToDoListId = data.ToDoListId,
            Tags = tags
        };

        return toDoModel;
    }

    public async Task<IEnumerable<ToDoModel>> GetToDosByTag(int tagId)
    {
        var toDos = await context.TagToDos
            .Where(x => x.TagsId == tagId)
            .Join(context.ToDos,
            tag => tag.ToDoId,
            toDo => toDo.Id,
            (tag, toDo) => toDo)
            .ToListAsync();

        var toDosModel = toDos.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,

        });

        return toDosModel;
    }

    public async Task<IEnumerable<ToDoModel>> SearchByType(int userId, string? search, string? searchType)
    {
        var query = context.ToDos
            .Where(param => param.UserId == userId);
        bool isValidDate = DateOnly.TryParseExact(search, "dd.MM.yyyy", out DateOnly date);

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

    public async Task<bool> IsToDoNameExists(string name, int listId)
        => await context.ToDos
        .AnyAsync(c => c.Name == name && c.ToDoListId == listId);

    private async Task<int> FindOrAddTagInDB(string name)
    {
        var tag = await context.Tags
                .Select(x => x)
                .FirstOrDefaultAsync(t => t.TagName == name);
        if (tag == null)
        {
            tag = new Tag { TagName = name };
            _ = await context.Tags.AddAsync(tag);
            _ = await context.SaveChangesAsync();
        }

        return tag.Id;
    }
}
