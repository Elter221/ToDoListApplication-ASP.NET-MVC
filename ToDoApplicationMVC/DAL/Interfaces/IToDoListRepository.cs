using ToDoApplicationMVC.DAL.Entities;

namespace ToDoApplicationMVC.DAL.Interfaces;

public interface IToDoListRepository : IRepository<ToDoList>
{
    IQueryable<ToDo> GetToDosOfList(int listId);
}
