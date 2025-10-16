using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.CustomAttributes;

namespace ToDoApplicationMVC.Models;

public class ToDoModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [Remote(action: "Validate", controller: "ToDo")]
    public string Name { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly CreatedAt { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [FutureDate(nameof(CreatedAt), ErrorMessage = "Deadline must be in the future")]
    public DateOnly Deadline { get; set; }

    [Required]
    public string Status { get; set; }

    public int ToDoListId { get; set; }

    public int UserId { get; set; }

    public ICollection<TagModel>? Tags { get; set; }

    public string TagsInput { get; set; } = string.Empty;
}
