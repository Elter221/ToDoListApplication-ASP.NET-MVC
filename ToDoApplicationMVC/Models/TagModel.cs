namespace ToDoApplicationMVC.Models;

public class TagModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<ToDoModel> ToDos { get; set; }
}
