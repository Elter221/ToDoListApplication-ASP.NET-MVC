using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.DAL;

public class TagRepository(TodoListDbContext context) : Repository<Tag>(context), ITagRepository
{
    public new async Task<int> Create(Tag model, CancellationToken cancellationToken = default)
    {
        if (await this.DbSet.AnyAsync(t => t.TagName == model.TagName, cancellationToken))
        {
            return -1;
        }

        return await base.Create(model, cancellationToken);
    }

    public async Task<bool> DeleteTagFromToDo(int tagId, int toDoId, CancellationToken cancellationToken = default)
    {
        var tag = await this.DbSet
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

    public new async Task<bool> Update(Tag model, CancellationToken cancellationToken = default)
    {
        if (await context.Tags.AnyAsync(c => c.TagName == model.TagName, cancellationToken))
        {
            return false;
        }

        return await base.Update(model, cancellationToken);
    }
}
