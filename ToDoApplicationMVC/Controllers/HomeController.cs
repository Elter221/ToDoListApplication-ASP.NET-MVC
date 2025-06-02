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
        var data = await context.ToDoLists.Include(x => x.ToDos).ToListAsync();

        var toDosModel = data.Select(x => new ToDoListModel()
        {
            Id = x.Id,
            Name = x.Name,
            CreatedAt = x.CreationDate,
            NumberOfTasks = x.ToDos.Count,
        }).ToArray();

        return this.View(toDosModel);
    }
}
