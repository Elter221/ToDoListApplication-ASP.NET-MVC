using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.DAL;

public class Repository<TEntity>(TodoListDbContext context) : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected DbSet<TEntity> DbSet { get; } = context.Set<TEntity>();
    public async Task<int> Create(TEntity model, CancellationToken cancellationToken = default)
    {
        await this.DbSet.AddAsync(model, cancellationToken);
        return (await this.DbSet.LastOrDefaultAsync(cancellationToken))!.Id;
    }

    public async Task Delete(int id, CancellationToken cancellationToken = default)
    {
        var data = await this.DbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (data != null)
        {
            this.DbSet.Remove(data);
        }
    }

    public IQueryable<TEntity> GetAll() => this.DbSet.Select(x => x);

    public async Task<TEntity?> GetById(int id, CancellationToken cancellationToken = default)
        => await this.DbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> Update(TEntity model, CancellationToken cancellationToken = default)
    {
        var data = await this.DbSet.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

        if (data != null)
        {
            data = model;
            return true;
        }

        return false;
    }
}
