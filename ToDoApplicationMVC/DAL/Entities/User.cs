using Microsoft.AspNetCore.Identity;

namespace ToDoApplicationMVC.DAL.Entities;

public class User : IdentityUser<int>
{
    public List<ToDo> ToDos { get; set; }
}
