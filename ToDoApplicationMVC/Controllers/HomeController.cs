using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DataAccess;
using ToDoApplicationMVC.Models;

namespace ToDoApplicationMVC.Controllers;
public class HomeController(TodoListDbContext context)
    : Controller
{
    public async Task<IActionResult> Index()
    {
        var data = await context.ToDos.ToListAsync();

        var toDosModel = data.Select(x => new ToDoModel()
        {
            Name = x.Name,
            Description = x.Description,
            CreatedAt = x.CreationDate,
            Deadline = x.Deadline,
            Status = x.Status.ToString(),
        }).ToArray();

        return View(toDosModel);
    }
}
