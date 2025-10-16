using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess;
using ToDoApplicationMVC.Models;

namespace ToDoApplicationMVC.Controllers;
public class TagController(TodoListDbContext context) : Controller
{
    public async Task<IActionResult> View()
    {
        var data = await context.Tags
            .ToListAsync();

        if (data == null)
        {
            return this.NotFound();
        }

        var TagModels = data.Select(x => new TagModel
        {
            Id = x.Id,
            Name = x.TagName,
        });

        return this.View(TagModels);
    }

    public async Task<IActionResult> OnTagClick(int tagId, string tagName)
    {
        var toDos = await context.TagToDos
            .Where(x => x.TagsId == tagId)
            .Join(context.ToDos,
            tag => tag.ToDoId,
            toDo => toDo.Id,
            (tag, toDo) => toDo)
            .ToListAsync();

        var toDosModel = toDos.Select(x => new ToDoModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
            ToDoListId = x.ToDoListId,

        });

        this.ViewBag.Tag = tagName;

        return this.View(toDosModel);
    }
}
