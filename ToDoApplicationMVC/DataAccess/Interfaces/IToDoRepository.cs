using ToDoApplicationMVC.DataAccess.Entities;

namespace ToDoApplicationMVC.DataAccess.Interfaces;

public interface IToDoRepository : IRepository<ToDo>
{
    Task AddTagToDo(int toDoId, string tagName, CancellationToken cancellationToken = default);
    Task<ToDo?> GetToDoWithTags(int id, CancellationToken cancellationToken = default);
    IQueryable<ToDo> GetToDosByTag(int tagId);
    IQueryable<ToDo> SearchByType(int userId, string? search, string? searchType);

    IQueryable<ToDo> SortByParams(int userId, string? sortBy, string? sortOrder);

}
