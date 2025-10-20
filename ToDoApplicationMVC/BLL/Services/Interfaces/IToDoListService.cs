using ToDoApplicationMVC.BLL.Models;

namespace ToDoApplicationMVC.BLL.Services.Interfaces;

public interface IToDoListService
{
    Task<IReadOnlyList<ToDoListModel>> GetToDoLists(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ToDoModel>> GetToDosOfList(int listId, CancellationToken cancellationToken = default);

    Task<bool> CreateToDoList(ToDoListModel model, CancellationToken cancellationToken = default);

    Task<ToDoListModel> GetToDoList(int id, CancellationToken cancellationToken = default);

    Task<bool> DeleteToDoList(int id, CancellationToken cancellationToken = default);

    Task<bool> EditToDoList(ToDoListModel model, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsycn(CancellationToken cancellationToken = default);
}
