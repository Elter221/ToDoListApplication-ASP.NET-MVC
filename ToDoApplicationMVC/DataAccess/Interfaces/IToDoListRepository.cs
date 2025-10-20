using ToDoApplicationMVC.DataAccess.Entities;

namespace ToDoApplicationMVC.DataAccess.Interfaces;

public interface IToDoListRepository : IRepository<ToDoList>
{
    IQueryable<ToDo> GetToDosOfList(int listId);
}
