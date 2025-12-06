namespace ToDoApplicationMVC.DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IToDoListRepository ToDoListRepository { get; }

    IToDoRepository ToDoRepository { get; }

    ITagRepository TagRepository { get; }

    ICommentRepository CommentRepository { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
