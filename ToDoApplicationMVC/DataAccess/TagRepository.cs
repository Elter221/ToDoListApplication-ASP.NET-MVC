using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess.Entities;
using ToDoApplicationMVC.DataAccess.Interfaces;

namespace ToDoApplicationMVC.DataAccess;

public class TagRepository(TodoListDbContext context) : ITagRepository
{
    public async Task<int> Create(Tag model, CancellationToken cancellationToken = default)
    {
        if (await context.Tags.AnyAsync(t => t.TagName == model.TagName, cancellationToken))
        {
            return -1;
        }

        await context.Tags.AddAsync(model, cancellationToken);
        return (await context.Tags.LastOrDefaultAsync(cancellationToken))!.Id;
    }

    public async Task Delete(int id, CancellationToken cancellationToken = default)
    {
        var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (tag != null)
        {
            context.Tags.Remove(tag);
        }
    }

    public async Task<bool> DeleteTagFromToDo(int tagId, int toDoId, CancellationToken cancellationToken = default)
    {
        var tag = await context.Tags
            .FirstOrDefaultAsync(t => t.Id == tagId, cancellationToken);

        if (tag is null)
        {
            return false;
        }

        var toDo = context.ToDos
            .Include(todo => todo.Tags)
            .SingleOrDefaultAsync(t => t.Id == toDoId, cancellationToken);
        if (toDo.Result != null)
        {
            toDo.Result.Tags.Remove(tag);
            return true;
        }


        return false;
    }

    public IQueryable<Tag> GetAll() => context.Tags.Select(x => x);

    public async Task<Tag?> GetById(int id, CancellationToken cancellationToken = default)
        => await context.Tags.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> Update(Tag model, CancellationToken cancellationToken = default)
    {
        if (await context.Tags.AnyAsync(c => c.TagName == model.TagName, cancellationToken))
        {
            return false;
        }

        var tagToFind = await context.Tags.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

        if (tagToFind != null)
        {
            tagToFind.TagName = model.TagName;
            return true;
        }

        return false;
    }
}
