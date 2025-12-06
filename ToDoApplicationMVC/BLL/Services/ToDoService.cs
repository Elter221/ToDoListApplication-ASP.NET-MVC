using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.BLL.Models;
using ToDoApplicationMVC.BLL.Services.Interfaces;
using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.BLL.Services;

public class ToDoService(IUnitOfWork unitOfWork) : IToDoService
{
    //Pass list id with model
    public async Task<bool> CreateNewToDoInList(ToDoModel model, CancellationToken cancellationToken = default)
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
            ToDoListId = model.ToDoListId,
            UserId = model.UserId,
        };

        if (await unitOfWork.ToDoRepository.Create(toDo, cancellationToken) < 0)
        {
            return false;
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
    {
        if (await unitOfWork.ToDoRepository.GetById(id, cancellationToken) == null)
        {
            return false;
        }

        await unitOfWork.ToDoRepository.Delete(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteTagFromToDo(int tagId, int toDoId, CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.TagRepository.DeleteTagFromToDo(tagId, toDoId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async Task<bool> EditToDo(ToDoModel toDo, CancellationToken cancellationToken = default)
    {
        ToDo model = new ToDo()
        {
            Id = toDo.Id,
            Name = toDo.Name,
            Description = toDo.Description,
            CreationDate = toDo.CreatedAt,
            Deadline = toDo.Deadline,
            Status = toDo.Status switch
            {
                "InProgress" => Status.InProgress,
                "Completed" => Status.Completed,
                _ => Status.Failed,
            },
            ToDoListId = toDo.ToDoListId,
            UserId = toDo.UserId,
        };

        var isEdited = await unitOfWork.ToDoRepository.Update(model, cancellationToken);

        await unitOfWork.ToDoRepository.AddTagToDo(toDo.Id, toDo.TagsInput, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return isEdited;
    }

    public async Task<IReadOnlyList<TagModel>> GetTags(CancellationToken cancellationToken = default)
    {
        var data = unitOfWork.TagRepository.GetAll();

        if (data == null)
        {
            return null!;
        }

        var tagModels = await data.Select(x => new TagModel
        {
            Id = x.Id,
            Name = x.TagName,
        }).ToArrayAsync(cancellationToken);

        return tagModels;
    }

    public async Task<IReadOnlyList<ToDoModel>> GetToDos(CancellationToken cancellationToken = default)
    {
        var data = unitOfWork.ToDoRepository.GetAll();

        if (data == null)
        {
            return null!;
        }

        var toDoModels = await data.Select(x => new ToDoModel
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,
            UserId = x.UserId,
        }).ToArrayAsync(cancellationToken);

        return toDoModels;
    }

    public async Task<ToDoModel?> GetToDoModelById(int id, CancellationToken cancellationToken = default)
    {
        var toDo = await unitOfWork.ToDoRepository.GetById(id, cancellationToken);
        if (toDo is null)
        {
            return null;
        }
        var toDoModel = new ToDoModel
        {
            Id = toDo.Id,
            Name = toDo.Name,
            Description = toDo.Description,
            CreatedAt = toDo.CreationDate,
            Deadline = toDo.Deadline,
            Status = toDo.Status.ToString(),
            ToDoListId = toDo.ToDoListId,
            UserId = toDo.UserId,
        };

        return toDoModel;
    }

    public async Task<ToDoModel?> GetToDoWithTagsAndComments(int id, CancellationToken cancellationToken = default)
    {
        var data = await unitOfWork.ToDoRepository.GetToDoWithTagsAndComments(id, cancellationToken);
        if (data == null)
        {
            return null;
        }

        var toDoModel = new ToDoModel()
        {
            Name = data.Name,
            Description = data.Description,
            CreatedAt = data.CreationDate,
            Deadline = data.Deadline,
            Status = data.Status.ToString(),
            ToDoListId = data.ToDoListId,
            Tags = data.Tags.Select(x => new TagModel
            {
                Id = x.Id,
                Name = x.TagName,
            }).ToList(),
            Comments = data.Comments.Select(x => new CommentModel
            {
                CommentId = x.Id,
                LastUpdateTime = x.LastUpdateTime,
                Description = x.Description,
            }).ToList()
        };

        return toDoModel;
    }

    public async Task<IReadOnlyList<ToDoModel>> GetToDosByTag(int tagId, CancellationToken cancellationToken = default)
    {
        var tagToDo = unitOfWork.ToDoRepository.GetToDosByTag(tagId);

        var toDosModel = await tagToDo.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,
            UserId = x.UserId,
        }).ToListAsync(cancellationToken);

        return toDosModel;
    }

    public async Task<IReadOnlyList<ToDoModel>> SearchByType(int userId, string? search, string? searchType, CancellationToken cancellationToken = default)
    {
        var data = await unitOfWork.ToDoRepository.SearchByType(userId, search, searchType).ToListAsync(cancellationToken);

        var toDosModel = data.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,
            UserId = x.UserId,
        }).ToArray();

        return toDosModel;
    }

    public async Task<IReadOnlyList<ToDoModel>> SortByParams(int userId, string? sortBy, string? sortOrder, CancellationToken cancellationToken = default)
    {
        var data = await unitOfWork.ToDoRepository.SortByParams(userId, sortBy, sortOrder).ToListAsync(cancellationToken);

        var toDosModel = data.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,
            UserId = x.UserId,
        }).ToArray();

        return toDosModel;
    }

    public async Task<bool> DeleteCommentFromToDo(int commentId, int toDoId, CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.CommentRepository.DeleteCommentFromToDo(commentId, toDoId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async Task<bool> AddToDoComment(string newComment, int toDoId, CancellationToken cancellationToken = default)
    {
        var comment = new Comment
        {
            Id = unitOfWork.CommentRepository.NextId,
            Description = newComment,
            ToDoId = toDoId,
            LastUpdateTime = DateTime.UtcNow,
        };

        var id = await unitOfWork.CommentRepository.Create(comment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return id >= 0;

    }

    public async Task<bool> EditCommentInToDo(int commentId, string newText, CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.CommentRepository.Update(commentId, newText, cancellationToken);
        if ((await unitOfWork.SaveChangesAsync(cancellationToken)) > 0 && result)
        {
            return true;
        }

        return false;
    }
}
