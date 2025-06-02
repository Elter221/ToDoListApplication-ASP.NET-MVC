using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [ForeignKey("ToDoList")]
    public int ToDoListId { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public ToDoList ToDoList { get; set; }

    public User User { get; set; }

}

public enum Status { InProgress, Completed, Failed }
