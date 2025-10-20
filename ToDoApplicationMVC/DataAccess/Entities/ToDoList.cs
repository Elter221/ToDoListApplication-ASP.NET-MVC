using System.ComponentModel.DataAnnotations;

namespace ToDoApplicationMVC.DataAccess.Entities;

public class ToDoList : BaseEntity
{
    [Required]
    public string Name { get; set; }

    [Required]
    public DateOnly CreationDate { get; set; }

    [Required]
    public int NumberOfTasks { get; set; }

    public List<ToDo> ToDos { get; set; }
}
