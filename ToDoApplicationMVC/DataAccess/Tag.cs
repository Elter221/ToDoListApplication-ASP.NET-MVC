using System.ComponentModel.DataAnnotations;

namespace ToDoApplicationMVC.DataAccess;

public class Tag
{
    public int Id { get; set; }

    public string TagName { get; set; }

    public List<ToDo> ToDos { get; set; }
}
