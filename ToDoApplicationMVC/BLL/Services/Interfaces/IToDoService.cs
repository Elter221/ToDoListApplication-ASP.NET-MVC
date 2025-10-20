using ToDoApplicationMVC.BLL.Models;

namespace ToDoApplicationMVC.BLL.Services.Interfaces;

public interface IToDoService
{
    Task<ToDoModel?> GetToDoWithTags(int id, CancellationToken cancellationToken = default);

    Task CreateNewToDoInList(ToDoModel model, CancellationToken cancellationToken = default);

    Task<ToDoModel?> GetToDoModelById(int id, CancellationToken cancellationToken = default);

    Task<bool> Delete(int id, CancellationToken cancellationToken = default);

    Task<bool> EditToDo(ToDoModel toDo, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ToDoModel>> SortByParams(int userId, string? sortBy, string? sortOrder, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ToDoModel>> SearchByType(int userId, string? search, string? searchType, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TagModel>> GetTags(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ToDoModel>> GetToDosByTag(int tagId, CancellationToken cancellationToken = default);

    Task<bool> DeleteTagFromToDo(int tagId, int toDoId, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsycn(CancellationToken cancellationToken = default);

}
