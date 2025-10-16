using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApplicationMVC.Models;

public class TagModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(15, ErrorMessage = "Name cannot exceed 15 characters")]
    [Remote(action: "Validate", controller: "Tag")]
    public string Name { get; set; }
}
