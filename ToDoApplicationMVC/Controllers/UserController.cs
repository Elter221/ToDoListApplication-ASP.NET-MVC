using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.DAL.Entities;

namespace ToDoApplicationMVC.Controllers;

public class UserController(UserManager<User> userManager, SignInManager<User> signInManager) : Controller
{

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string username)
    {
        var user = new User
        {
            UserName = username,
            Email = email
        };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "User");
            await signInManager.SignInAsync(user, isPersistent: true);
            return this.RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            this.ModelState.AddModelError("", error.Description);
        }

        return this.View();
    }

    [HttpGet]
    public IActionResult Login() => this.View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            this.ModelState.AddModelError("", "Invalid email or password");
            return this.View();
        }

        var result = await signInManager.PasswordSignInAsync(user.UserName!, password, true, false);

        if (result.Succeeded)
        {
            return this.RedirectToAction("Index", "Home");
        }

        this.ModelState.AddModelError("", "Invalid email or password");
        return this.View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return this.RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignNewEditor(string email)
    {
        var possiableUser = await userManager.FindByEmailAsync(email);
        if (possiableUser == null)
        {
            this.ModelState.AddModelError("", "No such user");
            return this.View();
        }

        await userManager.AddToRoleAsync(possiableUser, "Editor");
        return this.RedirectToAction("Index", "Home");
    }
}
