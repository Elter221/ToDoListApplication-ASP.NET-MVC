using ToDoApplicationMVC.DataAccess.Entities;
using ToDoApplicationMVC.DataAccess.Interfaces;

namespace ToDoApplicationMVC.DataAccess;

public class UnitOfWork(TodoListDbContext context) : IUnitOfWork
{
    private readonly TodoListDbContext context = context ?? throw new ArgumentNullException(nameof(context));

    public IToDoListRepository ToDoListRepository => new ToDoListRepository(this.context);

    public IToDoRepository ToDoRepository => new ToDoRepository(this.context);

    public ITagRepository TagRepository => new TagRepository(this.context);

    public IUserRepository UserRepository => new UserRepository(this.context);

    public ICommentRepository CommentRepository => new CommentRepository(this.context);

    public int SaveChanges() => this.context.SaveChanges();

    public void Dispose()
    {
        this.context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await this.context.SaveChangesAsync(cancellationToken);
}
