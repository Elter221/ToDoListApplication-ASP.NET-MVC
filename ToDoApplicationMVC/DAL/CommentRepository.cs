using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.DAL;

public class CommentRepository(TodoListDbContext context) : Repository<Comment>(context), ICommentRepository
{
    public new async Task<int> Create(Comment model, CancellationToken cancellationToken = default)
    {
        if (await this.DbSet.AnyAsync(c => c.Id == model.Id, cancellationToken))
        {
            return -1;
        }

        return await base.Create(model, cancellationToken);
    }

    public async Task<bool> DeleteCommentFromToDo(int commentId, int toDoId, CancellationToken cancellationToken = default)
    {
        var comment = await this.DbSet
            .FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken);

        if (comment is null)
        {
            return false;
        }

        var toDo = await context.ToDos
            .Include(comment => comment.Comments)
            .SingleOrDefaultAsync(t => t.Id == toDoId, cancellationToken);
        if (toDo != null)
        {
            toDo.Comments.Remove(comment);
            return true;
        }


        return false;
    }

    public async Task<bool> Update(int commentId, string newText, CancellationToken cancellationToken = default)
    {
        var data = await this.DbSet.FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken);
        if (data is null)
        {
            return false;
        }

        data.Description = newText;
        data.LastUpdateTime = DateTime.UtcNow;

        return true;
    }
}
