using Microsoft.AspNetCore.Mvc;

namespace ToDoApplicationMVC.Controllers;
public class CommentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
