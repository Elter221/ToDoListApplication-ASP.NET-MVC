using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.Services.Interfaces;

namespace ToDoApplicationMVC.Controllers;
public class TagController(IToDoService service) : Controller
{
    public async Task<IActionResult> View()
    {
        var result = await service.GetTags();
        if (result == null)
        {
            return this.NotFound();
        }

        return this.View(result);
    }

    public async Task<IActionResult> OnTagClick(int tagId, string tagName)
    {
        var toDosModel = await service.GetToDosByTag(tagId);

        this.ViewBag.Tag = tagName;

        return this.View("~/Views/ToDo/Index.cshtml", toDosModel);
    }

    [AcceptVerbs("POST")]
    public async Task<IActionResult> DeleteTag(int tagId, int id)
    {
        if (!await service.DeleteTag(tagId, id))
        {
            return this.NotFound();
        }

        return this.RedirectToAction("View", "ToDo", new { id });
    }
}
