using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess;
using ToDoApplicationMVC.Models;

namespace ToDoApplicationMVC.Controllers;
public class ToDoListController(TodoListDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> View([FromRoute] int id)
    {
        var data = await context.ToDoLists
            .Include(x => x.ToDos)
            .SingleOrDefaultAsync(x => x.Id == id);

        if (data == null)
        {
            return this.NotFound();
        }

        var toDosModel = data.ToDos.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = id,
        }).ToArray();

        this.ViewData["ListId"] = id;

        return this.View(toDosModel);
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

        if (await this.ListNameExists(model.Name))
        {
            this.ModelState.AddModelError(nameof(model.Name), "List name should be completly new");
            return this.View(model);
        }

        var toDoList = new ToDoList()
        {
            Name = model.Name,
            CreationDate = model.CreatedAt,
            NumberOfTasks = 0,
        };

        _ = context.ToDoLists.Add(toDoList);
        _ = await context.SaveChangesAsync();

        return this.RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var toDoList = await context.ToDoLists.FindAsync(id);
        if (toDoList is null)
        {
            return this.NotFound();
        }
        var toDoModel = new ToDoListModel
        {
            Id = toDoList.Id,
            Name = toDoList.Name,
            CreatedAt = toDoList.CreationDate,
            NumberOfTasks = toDoList.NumberOfTasks,
        };

        return this.View(toDoModel);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete([FromForm] int id)
    {
        var todoList = await context.ToDoLists
        .Include(t => t.ToDos)
        .SingleOrDefaultAsync(t => t.Id == id);

        if (todoList is null)
        {
            return this.NotFound();
        }

        context.ToDos.RemoveRange(todoList.ToDos);

        _ = context.ToDoLists.Remove(todoList);

        _ = await context.SaveChangesAsync();

        return this.RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int id)
    {
        var list = await context.ToDoLists.FindAsync(id);
        if (list is null)
        {
            return this.NotFound();
        }
        var vm = new ToDoListModel
        {
            Name = list.Name,
            CreatedAt = list.CreationDate,
            NumberOfTasks = list.NumberOfTasks,
        };
        return this.View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] ToDoListModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (await this.ListNameExists(model.Name))
        {
            this.ModelState.AddModelError(nameof(model.Name), "List name should be completly new");
            return this.View(model);
        }

        var listToFind = await context.ToDoLists.FirstAsync(x => x.Id == model.Id);

        if (listToFind == null)
        {
            return this.NotFound();
        }

        listToFind.Name = model.Name;
        listToFind.CreationDate = model.CreatedAt;

        _ = await context.SaveChangesAsync();

        return this.RedirectToAction("Index", "Home");
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<ActionResult> Validate(string name)
    {
        if (await this.ListNameExists(name))
        {
            return this.Json("List name already exists");
        }

        return this.Json(true);
    }

    private async Task<bool> ListNameExists(string name) =>
       await context.ToDoLists.AnyAsync(c => c.Name == name);

    private string CreateTagsStr(ICollection<Tag> tags)
    {
        StringBuilder tagsStr = new();
        if (tags != null)
        {
            foreach (var tag in tags)
            {
                _ = tagsStr.Append(tag.TagName + ", ");
            }
            _ = tagsStr.Remove(tagsStr.Length - 2, 2);
        }

        return tagsStr.ToString();
    }
}
