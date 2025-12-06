using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.DAL;

public class UnitOfWork(
    TodoListDbContext context,
    IToDoListRepository toDoListRepository,
    IToDoRepository toDoRepository,
    ITagRepository tagRepository,
    ICommentRepository commentRepository) : IUnitOfWork
{
    public IToDoListRepository ToDoListRepository => toDoListRepository;

    public IToDoRepository ToDoRepository => toDoRepository;

    public ITagRepository TagRepository => tagRepository;

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
