using ToDoApplicationMVC.BLL.Models;

namespace ToDoApplicationMVC.BLL.Services.Interfaces;

public interface IToDoService
{
    Task<ToDoModel?> GetToDoWithTagsAndComments(int id, CancellationToken cancellationToken = default);

    Task<bool> CreateNewToDoInList(ToDoModel model, CancellationToken cancellationToken = default);

    Task<ToDoModel?> GetToDoModelById(int id, CancellationToken cancellationToken = default);

    Task<bool> Delete(int id, CancellationToken cancellationToken = default);

    Task<bool> EditToDo(ToDoModel toDo, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ToDoModel>> SortByParams(int userId, string? sortBy, string? sortOrder, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ToDoModel>> SearchByType(int userId, string? search, string? searchType, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TagModel>> GetTags(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ToDoModel>> GetToDos(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ToDoModel>> GetToDosByTag(int tagId, CancellationToken cancellationToken = default);

    Task<bool> DeleteTagFromToDo(int tagId, int toDoId, CancellationToken cancellationToken = default);

    Task<bool> DeleteCommentFromToDo(int commentId, int toDoId, CancellationToken cancellationToken = default);

    Task<bool> AddToDoComment(string newComment, int toDoId, CancellationToken cancellationToken = default);

    Task<bool> EditCommentInToDo(int commentId, string newText, CancellationToken cancellationToken = default);
}
