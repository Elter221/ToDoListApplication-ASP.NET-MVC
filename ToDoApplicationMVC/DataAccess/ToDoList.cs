using System.ComponentModel.DataAnnotations;

namespace ToDoApplicationMVC.DataAccess;

public class ToDoList
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public DateOnly CreationDate { get; set; }

    [Required]
    public int NumberOfTasks { get; set; }

    public ICollection<ToDo> ToDos { get; set; }
}
