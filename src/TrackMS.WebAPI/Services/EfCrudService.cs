using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Abstractions;

namespace TrackMS.WebAPI.Services;

public class EfCrudService<TEntity, TKey> : ICrudService<TEntity, TKey> 
    where TEntity : class, IEntity<TKey>
{
    private readonly DbSet<TEntity> _dataSet;
    private readonly ApplicationDbContext _dbContext;

    public EfCrudService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dataSet = _dbContext.Set<TEntity>();
    }

    public virtual async Task CreateAsync(TEntity entity)
    { 
        _dataSet.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        _dataSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _dataSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(TKey id)
    {
        var entity = await _dataSet
            .FirstOrDefaultAsync(x => x.Id!.Equals(id));

        if (entity == null)
        {
            throw new ApplicationException($"'{typeof(TEntity).Name}' not found");
        }

        return entity;
    }

}
