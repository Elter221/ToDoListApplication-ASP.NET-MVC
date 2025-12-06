namespace ToDoApplicationMVC.BLL.Models;

public class CommentModel : BaseDTO
{
    public int CommentId { get; set; }
    public string Description { get; set; }

    public DateTime LastUpdateTime { get; set; }
}
