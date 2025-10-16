using Microsoft.AspNetCore.Mvc;
using ToDoApplicationMVC.Models;

namespace ToDoApplicationMVC.Services.Interfaces;

public interface IToDoService
{
    Task<ToDoModel> GetToDoWithTags(int id);

    Task CreateNewToDoInList(ToDoModel model, int listId);

    Task<ToDoModel> GetToDoModelById(int id, int listId);

    Task<bool> Delete(int id);

    Task<bool> EditToDo(ToDoModel toDo, int listId);

    Task<bool> IsToDoNameExists(string name, int listId);

    Task<IEnumerable<ToDoModel>> SortByParams(int userId, string? sortBy, string? sortOrder);

    Task<IEnumerable<ToDoModel>> SearchByType(int userId, string? search, string? searchType);

    Task<IEnumerable<TagModel>> GetTags();

    Task<IEnumerable<ToDoModel>> GetToDosByTag(int tagId);

    Task<bool> DeleteTag(int tagId, int toDoId);

    Task<bool> ValidateToDoName(string name, int listId);
}
