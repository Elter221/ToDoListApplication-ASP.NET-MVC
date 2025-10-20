using ToDoApplicationMVC.DataAccess.Entities;
using ToDoApplicationMVC.DataAccess.Interfaces;

namespace ToDoApplicationMVC.DataAccess;

public class CommentRepository(TodoListDbContext context) : ICommentRepository
{
    public Task<int> Create(Comment model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Comment> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Comment?> GetById(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(Comment model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
