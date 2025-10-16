using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.Models;
using ToDoApplicationMVC.Services.Interfaces;

namespace ToDoApplicationMVC.Controllers;
public class ToDoController(IToDoService service) : Controller
{
    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> Index([FromQuery] int userId, [FromForm] string? search = default, [FromForm] string? searchType = default)
    {
        var toDosModel = await service.SearchByType(userId, search, searchType);

        this.ViewBag.Search = search;

        return this.View(toDosModel);
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> Sort
        ([FromQuery] int userId, [FromForm] string? sortBy = default, [FromForm] string? sortOrder = default)
    {
        var toDosModel = await service.SortByParams(userId, sortBy, sortOrder);

        return this.View("Index", toDosModel);
    }

    public async Task<IActionResult> View([FromRoute] int id)
    {
        var toDoModel = await service.GetToDoWithTags(id);

        if (toDoModel == null)
        {
            return this.NotFound();
        }

        return this.View(toDoModel);
    }

    public IActionResult Create()
    {
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ToDoModel model, [FromRoute] int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (!await service.IsToDoNameExists(model.Name, id))
        {
            this.ModelState.AddModelError(nameof(model.Name), "ToDo name should be completly new");
            return this.View(model);
        }

        await service.CreateNewToDoInList(model, id);

        return this.RedirectToAction("View", "ToDoList", new { id });
    }

    public async Task<IActionResult> Delete([FromRoute] int id, [FromQuery] int listid)
    {
        var toDoModel = await service.GetToDoModelById(id, listid);
        if (toDoModel is null)
        {
            return this.NotFound();
        }


        return this.View(toDoModel);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete([FromForm] int id, [FromForm] int listid)
    {
        if (!await service.Delete(id))
        {
            return this.NotFound();
        }

        id = listid;

        return this.RedirectToAction("View", "ToDoList", new { id });
    }

    public async Task<IActionResult> Edit([FromRoute] int id, [FromQuery] int listid)
    {
        var toDoModel = await service.GetToDoModelById(id, listid);
        if (toDoModel is null)
        {
            return this.NotFound();
        }


        return this.View(toDoModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] ToDoModel model, [FromForm] int listid)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (!await service.EditToDo(model, listid))
        {
            this.ModelState.AddModelError(nameof(model.Name), "ToDo name should be completly new");
            return this.View(model);
        }

        int id = listid;

        return this.RedirectToAction("View", "ToDoList", new { id });
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<ActionResult> Validate(string name, int listid)
    {
        if (await service.ValidateToDoName(name, listid))
        {
            return this.Json("ToDo name already exists");
        }

        return this.Json(true);
    }
}
