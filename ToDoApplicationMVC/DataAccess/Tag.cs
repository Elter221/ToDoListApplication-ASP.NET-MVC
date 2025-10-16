using System.ComponentModel.DataAnnotations;

namespace ToDoApplicationMVC.DataAccess;

public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string TagName { get; set; }

    public ICollection<ToDo> ToDos { get; set; }
}
