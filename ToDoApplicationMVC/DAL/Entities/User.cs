namespace ToDoApplicationMVC.DAL.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public List<ToDo> ToDos { get; set; }
}
