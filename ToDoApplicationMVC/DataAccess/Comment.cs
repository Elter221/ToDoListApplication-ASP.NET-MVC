using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApplicationMVC.DataAccess;

public class Comment
{
    [Key]
    public int Id { get; set; }

    public string Description { get; set; }

    public DateTime LastUpdateTime { get; set; }

    [ForeignKey("ToDo")]
    public int ToDoId { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public ToDo ToDo { get; set; }

    public User User { get; set; }
}
