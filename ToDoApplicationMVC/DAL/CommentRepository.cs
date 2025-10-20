using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.DAL;

public class CommentRepository(TodoListDbContext context) : Repository<Comment>(context), ICommentRepository
{
}
