using ToDoApplicationMVC.DataAccess.Entities;

namespace ToDoApplicationMVC.DataAccess.Interfaces;

public interface IRepository<T>
    where T : BaseEntity
{
    IQueryable<T> GetAll();
    Task<T?> GetById(int id, CancellationToken cancellationToken = default);
    Task<int> Create(T model, CancellationToken cancellationToken = default);
    Task<bool> Update(T model, CancellationToken cancellationToken = default);
    Task Delete(int id, CancellationToken cancellationToken = default);
}
