using Microsoft.EntityFrameworkCore;

namespace ToDoApplicationMVC.DataAccess;

[PrimaryKey(nameof(TagsId), nameof(ToDoId))]
public class TagToDo
{
    public int TagsId { get; set; }

    public int ToDoId { get; set; }
}
