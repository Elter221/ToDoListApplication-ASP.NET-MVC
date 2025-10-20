using ToDoApplicationMVC.DataAccess.Entities;

namespace ToDoApplicationMVC.DataAccess.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
    Task<bool> DeleteTagFromToDo(int tagId, int toDoId, CancellationToken cancellationToken = default);
}
