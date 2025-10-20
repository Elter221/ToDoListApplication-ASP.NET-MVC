namespace ToDoApplicationMVC.Models;

public class TagModel : BaseDTO
{
    public string Name { get; set; }

    public List<ToDoModel> ToDos { get; set; }
}
