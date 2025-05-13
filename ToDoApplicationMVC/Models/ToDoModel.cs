namespace ToDoApplicationMVC.Models;

public class ToDoModel
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly Deadline { get; set; }

    public string Status { get; set; }
}
