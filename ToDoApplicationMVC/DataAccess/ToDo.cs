using System.ComponentModel.DataAnnotations;

namespace ToDoApplicationMVC.DataAccess;

public class ToDo
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public DateOnly CreationDate { get; set; }

    [Required]
    public DateOnly Deadline { get; set; }

    [Required]
    public Status Status { get; set; }
}

public enum Status { InProgress, Completed, Failed }
