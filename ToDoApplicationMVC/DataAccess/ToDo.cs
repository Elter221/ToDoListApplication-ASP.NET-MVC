using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApplicationMVC.DataAccess;

public class ToDo
{
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

    public int ToDoListId { get; set; }

    public int UserId { get; set; }

    public ToDoList ToDoList { get; set; }

    public User User { get; set; }

    public List<Tag> Tags { get; set; }

    public List<Comment> Comments { get; set; }

}

public enum Status { InProgress, Completed, Failed }
