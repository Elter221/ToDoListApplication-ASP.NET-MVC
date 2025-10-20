using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.BLL.Models;
using ToDoApplicationMVC.BLL.Services.Interfaces;
using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.BLL.Services;

public class ToDoListService(IUnitOfWork unitOfWork) : IToDoListService
{
    public async Task<bool> CreateToDoList(ToDoListModel model, CancellationToken cancellationToken = default)
    {
        var toDoList = new ToDoList()
        {
            Name = model.Name,
            CreationDate = model.CreatedAt,
            NumberOfTasks = 0,
        };

        var result = (await unitOfWork.ToDoListRepository.Create(toDoList, cancellationToken)) > 0;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }

    public async Task<bool> DeleteToDoList(int id, CancellationToken cancellationToken = default)
    {
        if (await unitOfWork.ToDoListRepository.GetById(id, cancellationToken) == null)
        {
            return false;
        }

        foreach (var item in await unitOfWork.ToDoListRepository.GetToDosOfList(id).ToListAsync(cancellationToken))
        {
            await unitOfWork.ToDoRepository.Delete(item.Id, cancellationToken);
        }
        await unitOfWork.ToDoListRepository.Delete(id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> EditToDoList(ToDoListModel model, CancellationToken cancellationToken = default)
    {
        ToDoList dto = new ToDoList
        {
            Id = model.Id,
            Name = model.Name,
            CreationDate = model.CreatedAt,
            NumberOfTasks = 0,
        };
        var result = await unitOfWork.ToDoListRepository.Update(dto, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }

    public async Task<IReadOnlyList<ToDoListModel>> GetToDoLists(CancellationToken cancellationToken = default)
    {
        var models = await unitOfWork.ToDoListRepository.GetAll()
            .Select(x => new ToDoListModel
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreationDate,
                NumberOfTasks = x.NumberOfTasks,
            }).ToArrayAsync(cancellationToken);
        return models;
    }

    public async Task<ToDoListModel> GetToDoList(int id, CancellationToken cancellationToken = default)
    {
        var toDoList = await unitOfWork.ToDoListRepository.GetById(id, cancellationToken);
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

    public async Task<IReadOnlyList<ToDoModel>> GetToDosOfList(int listId, CancellationToken cancellationToken = default)
    {
        var data = unitOfWork.ToDoListRepository.GetToDosOfList(listId);

        if (data == null)
        {
            return null!;
        }

        var toDosModel = await data.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = listId,
        }).ToArrayAsync(cancellationToken);

        return toDosModel;
    }
}
