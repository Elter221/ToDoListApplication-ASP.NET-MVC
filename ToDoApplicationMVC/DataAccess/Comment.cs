namespace ToDoApplicationMVC.DataAccess;

public class Comment
{
    public int Id { get; set; }

    public string Description { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public int ToDoId { get; set; }

    public int UserId { get; set; }

    public ToDo ToDo { get; set; }
}
