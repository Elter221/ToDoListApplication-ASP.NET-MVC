using ToDoApplicationMVC.DataAccess.Entities;

namespace ToDoApplicationMVC.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<ToDoList> ToDoListRepository { get; }

    IRepository<ToDo> ToDoRepository { get; }

    IRepository<Tag> TagRepository { get; }

    IRepository<User> UserRepository { get; }

    IRepository<Comment> CommentRepository { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
