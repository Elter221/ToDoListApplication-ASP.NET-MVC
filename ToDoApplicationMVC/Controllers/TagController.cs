using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.BLL.Services.Interfaces;

namespace ToDoApplicationMVC.Controllers;
public class TagController(IToDoService service) : Controller
{
    public async Task<IActionResult> View(CancellationToken cancellationToken = default)
    {
        var result = await service.GetTags(cancellationToken);
        if (result == null)
        {
            return this.NotFound();
        }

        return this.View(result);
    }

    public async Task<IActionResult> OnTagClick(int tagId, string tagName, CancellationToken cancellationToken = default)
    {
        var toDosModel = await service.GetToDosByTag(tagId, cancellationToken);

        this.ViewBag.Tag = tagName;

        return this.View("~/Views/ToDo/Index.cshtml", toDosModel);
    }

    [AcceptVerbs("POST")]
    public async Task<IActionResult> DeleteTag(int tagId, int id, CancellationToken cancellationToken = default)
    {
        if (!await service.DeleteTagFromToDo(tagId, id, cancellationToken))
        {
            return this.NotFound();
        }

        return this.RedirectToAction("View", "ToDo", new { id });
    }
}
