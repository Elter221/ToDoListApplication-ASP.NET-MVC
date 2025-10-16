using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess;
using ToDoApplicationMVC.Models;

namespace ToDoApplicationMVC.Controllers;
public class ToDoController(TodoListDbContext context) : Controller
{
    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> Index([FromQuery] int userId, [FromForm] string? search = default, [FromForm] string? searchType = default)
    {
        var query = context.ToDos
            .Where(param => param.UserId == userId);
        bool isValidDate = DateOnly.TryParseExact(search, "dd.MM.yyyy", out DateOnly date);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = searchType switch
            {
                "name" => query.Where(c => c.Name.Contains(search)),
                "status" => query.Where(c => c.Status.ToString() == search),
                "deadline" => isValidDate ? query.Where(c => c.Deadline <= date) : query.Where(c => false),
                _ => query,
            };
        }

        var data = await query.ToListAsync();

        var toDosModel = data.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,
        }).ToArray();

        this.ViewBag.Search = search;

        return this.View(toDosModel);
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> Sort
        ([FromQuery] int userId, [FromForm] string? sortBy = default, [FromForm] string? sortOrder = default)
    {
        var query = context.ToDos
            .Where(param => param.UserId == userId);

        query = (sortBy?.ToLower(), sortOrder?.ToLower()) switch
        {

            ("name", "asc") => query.OrderBy(t => t.Name),
            ("name", "desc") => query.OrderByDescending(t => t.Name),

            ("status", "asc") => query.OrderBy(t => t.Status),
            ("status", "desc") => query.OrderByDescending(t => t.Status),

            ("deadline", "asc") => query.OrderBy(t => t.Deadline),
            ("deadline", "desc") => query.OrderByDescending(t => t.Deadline),

            (_, "asc") => query.OrderBy(t => t.CreationDate),
            (_, "desc") => query.OrderByDescending(t => t.CreationDate),

            _ => query
        };

        var data = await query.ToListAsync();

        var toDosModel = data.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,
        }).ToArray();

        return this.View("Index", toDosModel);
    }

    public async Task<IActionResult> View([FromRoute] int id)
    {
        var data = await context.ToDos
            .FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
        {
            return this.NotFound();
        }

        var tags = await context.TagToDos
                .Where(x => x.ToDoId == data.Id)
                .Join(context.Tags,
                    tagToDo => tagToDo.TagsId,
                    tag => tag.Id,
                    (tagToDo, tag) => tag)
                .Select(x => new TagModel()
                {
                    Id = x.Id,
                    Name = x.TagName
                })
                .ToListAsync();
        // в маппинг вынести
        var toDosModel = new ToDoModel()
        {
            Name = data.Name,
            Description = data.Description,
            CreatedAt = data.CreationDate,
            Deadline = data.Deadline,
            Status = data.Status.ToString(),
            ToDoListId = data.ToDoListId,
            Tags = tags
        };

        return this.View(toDosModel);
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

        if (await this.ToDoNameExists(model.Name, id))
        {
            this.ModelState.AddModelError(nameof(model.Name), "ToDo name should be completly new");
            return this.View(model);
        }

        var toDo = new ToDo()
        {
            Name = model.Name,
            Description = model.Description,
            CreationDate = model.CreatedAt,
            Deadline = model.Deadline,
            Status = model.Status switch
            {
                "InProgress" => Status.InProgress,
                "Completed" => Status.Completed,
                _ => Status.Failed,
            },
            ToDoListId = id,
            UserId = (await context.Users.FirstAsync()).Id,
        };

        _ = context.ToDos.Add(toDo);
        _ = await context.SaveChangesAsync();

        return this.RedirectToAction("View", "ToDoList", new { id });
    }

    public async Task<IActionResult> Delete([FromRoute] int id, [FromQuery] int listid)
    {
        var toDo = await context.ToDos.FindAsync(id);
        if (toDo is null)
        {
            return this.NotFound();
        }
        var toDoModel = new ToDoModel
        {
            Id = toDo.Id,
            Name = toDo.Name,
            Description = toDo.Description,
            CreatedAt = toDo.CreationDate,
            Deadline = toDo.Deadline,
            Status = toDo.Status.ToString(),
            ToDoListId = listid,
        };

        return this.View(toDoModel);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete([FromForm] int id, [FromForm] int listid)
    {
        var country = await context.ToDos.FindAsync(id);

        if (country is null)
        {
            return this.NotFound();
        }

        _ = context.ToDos.Remove(country);

        _ = await context.SaveChangesAsync();

        id = listid;

        return this.RedirectToAction("View", "ToDoList", new { id });
    }

    public async Task<IActionResult> Edit([FromRoute] int id, [FromQuery] int listid)
    {
        var toDo = await context.ToDos.FindAsync(id);
        if (toDo is null)
        {
            return this.NotFound();
        }
        var vm = new ToDoModel
        {
            Name = toDo.Name,
            Description = toDo.Description,
            CreatedAt = toDo.CreationDate,
            Deadline = toDo.Deadline,
            Status = toDo.Status.ToString(),
            ToDoListId = listid,
        };

        return this.View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] ToDoModel model, [FromForm] int listid)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (await this.ToDoNameExists(model.Name, listid))
        {
            this.ModelState.AddModelError(nameof(model.Name), "ToDo name should be completly new");
            return this.View(model);
        }

        var toDoToFind = await context.ToDos.FirstAsync(x => x.Id == model.Id);

        if (toDoToFind == null)
        {
            return this.NotFound();
        }

        toDoToFind.Name = model.Name;
        toDoToFind.Description = model.Description;
        toDoToFind.CreationDate = model.CreatedAt;
        toDoToFind.Deadline = model.Deadline;
        toDoToFind.Status = model.Status switch
        {
            "InProgress" => Status.InProgress,
            "Completed" => Status.Completed,
            _ => Status.Failed,
        };
        if (model.TagsInput != string.Empty)
        {
            var tagId = await this.FindOrAddTagInDB(model.TagsInput);
            if (!context.TagToDos.Any(x => x.TagsId == tagId && x.ToDoId == toDoToFind.Id))
            {
                await context.TagToDos!.AddAsync(
                new TagToDo
                {
                    TagsId = tagId,
                    ToDoId = toDoToFind.Id
                });
            }
        }

        await context.SaveChangesAsync();

        int id = listid;

        return this.RedirectToAction("View", "ToDoList", new { id });
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<ActionResult> Validate(string name, int listid)
    {
        if (await this.ToDoNameExists(name, listid))
        {
            return this.Json("ToDo name already exists");
        }

        return this.Json(true);
    }

    private async Task<bool> ToDoNameExists(string name, int id) =>
       await context.ToDos.AnyAsync(c => c.Name == name && c.ToDoListId == id);

    private async Task<int> FindOrAddTagInDB(string name)
    {
        var tag = await context.Tags
                .Select(x => x)
                .FirstOrDefaultAsync(t => t.TagName == name);
        if (tag == null)
        {
            tag = new Tag { TagName = name };
            _ = await context.Tags.AddAsync(tag);
            _ = await context.SaveChangesAsync();
        }

        return tag.Id;
    }

}
