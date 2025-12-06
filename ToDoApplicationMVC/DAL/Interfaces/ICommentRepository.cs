using ToDoApplicationMVC.DAL.Entities;

namespace ToDoApplicationMVC.DAL.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<bool> DeleteCommentFromToDo(int commentId, int toDoId, CancellationToken cancellationToken = default);

}
