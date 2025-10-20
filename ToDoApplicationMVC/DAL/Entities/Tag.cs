using System.ComponentModel.DataAnnotations;
using ToDoApplicationMVC.DAL.Entities;

namespace ToDoApplicationMVC.DAL.Entities;

public class Tag : BaseEntity
{
    public string TagName { get; set; }

    public List<ToDo> ToDos { get; set; }
}
