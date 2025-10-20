using ToDoApplicationMVC.DAL.Entities;

namespace ToDoApplicationMVC.DAL.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
    Task<bool> DeleteTagFromToDo(int tagId, int toDoId, CancellationToken cancellationToken = default);
}
