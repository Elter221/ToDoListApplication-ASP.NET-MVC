using ToDoApplicationMVC.DAL.Entities;

namespace ToDoApplicationMVC.DAL.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<bool> DeleteCommentFromToDo(int commentId, int toDoId, CancellationToken cancellationToken = default);

    Task<bool> Update(int commentId, string newText, CancellationToken cancellationToken = default);

}
