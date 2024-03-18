namespace TrackMS.Domain.Abstractions;

public interface ICrudService<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public Task CreateAsync(TEntity entity);

    public Task UpdateAsync(TEntity entity);
    
    public Task DeleteAsync(TEntity entity);

    public Task<TEntity> GetByIdAsync(TKey id);
}
