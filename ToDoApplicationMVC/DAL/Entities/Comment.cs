namespace ToDoApplicationMVC.DataAccess.Entities;

public class Comment : BaseEntity
{
    public string Description { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public int ToDoId { get; set; }

    public int UserId { get; set; }

    public ToDo ToDo { get; set; }
}
