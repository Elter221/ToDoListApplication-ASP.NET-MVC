namespace ToDoApplicationMVC.Models;

public class TagModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<ToDoModel> ToDos { get; set; }
}
