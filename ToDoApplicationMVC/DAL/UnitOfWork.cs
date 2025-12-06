using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.DAL;

public class UnitOfWork(
    TodoListDbContext context,
    IToDoListRepository toDoListRepository,
    IToDoRepository toDoRepository,
    IUserRepository userRepository,
    ITagRepository tagRepository,
    ICommentRepository commentRepository) : IUnitOfWork
{
    public IToDoListRepository ToDoListRepository => toDoListRepository;

    public IToDoRepository ToDoRepository => toDoRepository;

    public ITagRepository TagRepository => tagRepository;

    public IUserRepository UserRepository => userRepository;

    public ICommentRepository CommentRepository => commentRepository;

    public int SaveChanges() => context.SaveChanges();

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}
