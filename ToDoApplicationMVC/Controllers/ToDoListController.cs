using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.Models;
using ToDoApplicationMVC.Services.Interfaces;

namespace ToDoApplicationMVC.Controllers;
public class ToDoListController(IToDoListService service) : Controller
{
    [HttpGet]
    public async Task<IActionResult> View([FromRoute] int id)
    {
        var data = await service.GetToDosOfList(id);

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
    public async Task<IActionResult> Create([FromForm] ToDoListModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (!await service.AddNewToDoList(model))
        {
            this.ModelState.AddModelError(nameof(model.Name), "List name should be completly new");
            return this.View(model);
        }

        return this.RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var toDoList = await service.GetToDoList(id);
        if (toDoList is null)
        {
            return this.NotFound();
        }

        return this.View(toDoList);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete([FromForm] int id)
    {
        if (!await service.DeleteToDoList(id))
        {
            return this.NotFound();
        }

        return this.RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int id)
    {
        var toDoList = await service.GetToDoList(id);
        if (toDoList is null)
        {
            return this.NotFound();
        }

        return this.View(toDoList);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] ToDoListModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (!await service.EditToDoList(model))
        {
            this.ModelState.AddModelError(nameof(model.Name), "List name should be completly new");
            return this.View(model);
        }

        return this.RedirectToAction("Index", "Home");
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<ActionResult> Validate(string name)
    {
        if (await service.ListNameExists(name))
        {
            return this.Json("List name already exists");
        }

        return this.Json(true);
    }
}
