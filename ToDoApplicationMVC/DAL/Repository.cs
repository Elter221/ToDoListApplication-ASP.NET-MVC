using Microsoft.EntityFrameworkCore;
using ToDoApplicationMVC.DAL.Entities;
using ToDoApplicationMVC.DAL.Interfaces;

namespace ToDoApplicationMVC.DAL;

public class Repository<TEntity>(TodoListDbContext context) : IRepository<TEntity>
    where TEntity : BaseEntity
{
    public int NextId { get; set; } = 0;
    protected DbSet<TEntity> DbSet { get; } = context.Set<TEntity>();
    public async Task<int> Create(TEntity model, CancellationToken cancellationToken = default)
    {
        var result = await this.DbSet.AddAsync(model, cancellationToken);
        this.NextId = result.Entity.Id + 1;
        return result.Entity.Id;
    }

    public async Task Delete(int id, CancellationToken cancellationToken = default)
    {
        var data = await this.DbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (data != null)
        {
            this.DbSet.Remove(data);
        }
    }

    public IQueryable<TEntity> GetAll() => this.DbSet;

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
