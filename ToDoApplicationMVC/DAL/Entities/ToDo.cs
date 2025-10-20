using System.ComponentModel.DataAnnotations;
using ToDoApplicationMVC.DAL.Entities;

namespace ToDoApplicationMVC.DAL.Entities;

public class ToDo : BaseEntity
{
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
