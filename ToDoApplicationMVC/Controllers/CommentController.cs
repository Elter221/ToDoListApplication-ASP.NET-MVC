using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.BLL.Services.Interfaces;

namespace ToDoApplicationMVC.Controllers;
public class CommentController(IToDoService service) : Controller
{

    [AcceptVerbs("POST")]
    public async Task<IActionResult> AddComment(string newComment, int id, CancellationToken cancellationToken = default)
    {
        if (!await service.AddToDoComment(newComment, id, cancellationToken))
        {
            return this.NotFound();
        }

        return this.RedirectToAction("View", "ToDo", new { id });
    }

    [AcceptVerbs("POST")]
    public async Task<IActionResult> DeleteComment(int commentId, int id, CancellationToken cancellationToken = default)
    {
        if (!await service.DeleteCommentFromToDo(commentId, id, cancellationToken))
        {
            return this.NotFound();
        }

        return this.RedirectToAction("View", "ToDo", new { id });
    }

    [AcceptVerbs("POST")]
    public async Task<IActionResult> EditComment(int commentId, string newText, int id, CancellationToken cancellationToken = default)
    {
        if (!await service.EditCommentInToDo(commentId, newText, cancellationToken))
        {
            return this.NotFound();
        }

        return this.RedirectToAction("View", "ToDo", new { id });
    }
}
