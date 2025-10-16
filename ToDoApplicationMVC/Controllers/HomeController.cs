using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.Services.Interfaces;

namespace ToDoApplicationMVC.Controllers;
public class HomeController(IToDoListService service)
    : Controller
{
    public async Task<IActionResult> Index()
        => this.View(await service.GetToDoLists());
}
