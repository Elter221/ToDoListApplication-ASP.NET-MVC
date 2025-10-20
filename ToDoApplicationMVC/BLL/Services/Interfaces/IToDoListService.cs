using ToDoApplicationMVC.BLL.Models;

namespace ToDoApplicationMVC.BLL.Services.Interfaces;

public interface IToDoListService
{
    Task<IEnumerable<ToDoListModel>> GetToDoLists();
    Task<IEnumerable<ToDoModel>> GetToDosOfList(int listId);
    Task<bool> ListNameExists(string name);

    Task<bool> AddNewToDoList(ToDoListModel model);

    Task<ToDoListModel> GetToDoList(int id);

    Task<bool> DeleteToDoList(int id);

    Task<bool> EditToDoList(ToDoListModel model);
}
