using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApplicationMVC.BLL.Models;

public class ToDoListModel : BaseDTO
{
    [Required]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [Remote(action: "Validate", controller: "ToDoList")]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly CreatedAt { get; set; }

    public int NumberOfTasks { get; set; }
}
