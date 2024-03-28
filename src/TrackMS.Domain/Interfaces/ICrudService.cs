using TrackMS.Domain.ServiceResultAPI;

namespace TrackMS.Domain.Interfaces;

public interface ICrudService<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public Task<ServiceResult> CreateAsync(TEntity entity);

    public Task<ServiceResult> UpdateAsync(TEntity entity);

    public Task<ServiceResult> DeleteAsync(TEntity entity);

    public Task<ObjectServiceResult<TEntity>> GetByIdAsync(TKey id);

    public IQueryable<TEntity> GetEntities();
}
