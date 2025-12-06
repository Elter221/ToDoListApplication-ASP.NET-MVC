using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.BLL.Services.Interfaces;

namespace ToDoApplicationMVC.Controllers;

[Authorize]
public class HomeController(IToDoListService service)
    : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        => this.View(await service.GetToDoLists(cancellationToken));
}
