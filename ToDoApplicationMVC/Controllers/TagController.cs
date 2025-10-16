using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess;
using ToDoApplicationMVC.Models;

namespace ToDoApplicationMVC.Controllers;
public class TagController(TodoListDbContext context) : Controller
{
    public async Task<IActionResult> View()
    {
        // get Tags with connected Todos
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
}
