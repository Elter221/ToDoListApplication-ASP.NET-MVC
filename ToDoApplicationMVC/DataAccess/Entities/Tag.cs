using System.ComponentModel.DataAnnotations;

namespace ToDoApplicationMVC.DataAccess.Entities;

public class Tag : BaseEntity
{
    public string TagName { get; set; }

    public List<ToDo> ToDos { get; set; }
}
