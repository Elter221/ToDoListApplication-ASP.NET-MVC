using System.ComponentModel.DataAnnotations;

namespace ToDoApplicationMVC.DataAccess;

public class ToDoList
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public DateOnly CreationDate { get; set; }

    [Required]
    public int NumberOfTasks { get; set; }

    public List<ToDo> ToDos { get; set; }
}
