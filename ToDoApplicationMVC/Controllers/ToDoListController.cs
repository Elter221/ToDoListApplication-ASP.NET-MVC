using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.BLL.Models;
using ToDoApplicationMVC.BLL.Services.Interfaces;

namespace ToDoApplicationMVC.Controllers;

[Authorize]
public class ToDoListController(IToDoListService service) : Controller
{
    [HttpGet]
    public async Task<IActionResult> View([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var data = await service.GetToDosOfList(id, cancellationToken);

        if (data == null)
        {
            return this.NotFound();
        }

        this.ViewData["ListId"] = id;

        return this.View(data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return this.View();
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ToDoListModel model, CancellationToken cancellationToken = default)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (!await service.CreateToDoList(model, cancellationToken))
        {
            this.ModelState.AddModelError(nameof(model.Name), "List name should be completly new");
            return this.View(model);
        }

        return this.RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var toDoList = await service.GetToDoList(id, cancellationToken);
        if (toDoList is null)
        {
            return this.NotFound();
        }

        return this.View(toDoList);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete([FromForm] int id, CancellationToken cancellationToken = default)
    {
        if (!await service.DeleteToDoList(id, cancellationToken))
        {
            return this.NotFound();
        }

        return this.RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var toDoList = await service.GetToDoList(id, cancellationToken);
        if (toDoList is null)
        {
            return this.NotFound();
        }

        return this.View(toDoList);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] ToDoListModel model, CancellationToken cancellationToken = default)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (!await service.EditToDoList(model, cancellationToken))
        {
            this.ModelState.AddModelError(nameof(model.Name), "List name should be completly new");
            return this.View(model);
        }

        return this.RedirectToAction("Index", "Home");
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<ActionResult> Validate(string name, CancellationToken cancellationToken = default)
    {
        if ((await service.GetToDoLists(cancellationToken)).Any(x => x.Name == name))
        {
            return this.Json("List name already exists");
        }

        return this.Json(true);
    }
}
