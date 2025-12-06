using System.Threading;
using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.BLL.Models;
using ToDoApplicationMVC.BLL.Services.Interfaces;

namespace ToDoApplicationMVC.Controllers;
public class ToDoController(IToDoService service) : Controller
{
    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> Index(
        [FromQuery] int userId,
        [FromForm] string? search = default,
        [FromForm] string? searchType = default,
        CancellationToken cancellationToken = default)
    {
        var toDosModel = await service.SearchByType(userId, search, searchType, cancellationToken);

        this.ViewBag.Search = search;

        return this.View(toDosModel);
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> Sort
        ([FromQuery] int userId,
        [FromForm] string? sortBy = default,
        [FromForm] string? sortOrder = default,
        CancellationToken cancellationToken = default)
    {
        var toDosModel = await service.SortByParams(userId, sortBy, sortOrder, cancellationToken);

        return this.View("Index", toDosModel);
    }

    public async Task<IActionResult> View([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var toDoModel = await service.GetToDoWithTags(id, cancellationToken);

        if (toDoModel == null)
        {
            //статус код страницы
            return this.NotFound();
        }

        return this.View(toDoModel);
    }

    public IActionResult Create()
    {
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ToDoModel model, [FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }
        model.ToDoListId = id;
        model.UserId = 1;

        if (!await service.CreateNewToDoInList(model, cancellationToken))
        {
            this.ModelState.AddModelError(nameof(model.Name), "ToDo name should be completly new");
            return this.View(model);
        }

        return this.RedirectToAction("View", "ToDoList", new { id });
    }

    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var toDoModel = await service.GetToDoModelById(id, cancellationToken);
        if (toDoModel is null)
        {
            return this.NotFound();
        }


        return this.View(toDoModel);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete([FromForm] int id, [FromForm] int listid, CancellationToken cancellationToken = default)
    {
        if (!await service.Delete(id, cancellationToken))
        {
            return this.NotFound();
        }

        return this.RedirectToAction("View", "ToDoList", new { id = listid });
    }

    public async Task<IActionResult> Edit([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var toDoModel = await service.GetToDoModelById(id, cancellationToken);
        if (toDoModel is null)
        {
            return this.NotFound();
        }

        return this.View(toDoModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(
        [FromForm] ToDoModel model,
        [FromForm] int listid,
        CancellationToken cancellationToken = default)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (!await service.EditToDo(model, cancellationToken))
        {
            this.ModelState.AddModelError(nameof(model.Name), "ToDo name should be completly new");
            return this.View(model);
        }

        return this.RedirectToAction("View", "ToDoList", new { id = listid });
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<ActionResult> Validate(string name, int listid, CancellationToken cancellationToken = default)
    {
        if ((await service.GetToDos(cancellationToken))
            .Any(x => x.Name == name && x.ToDoListId == listid))
        {
            return this.Json("ToDo name already exists");
        }

        return this.Json(true);
    }
}
