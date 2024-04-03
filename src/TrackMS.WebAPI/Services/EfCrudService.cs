using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Interfaces;
using TrackMS.Domain.ServiceResultAPI;

namespace TrackMS.WebAPI.Services;

public class EfCrudService<TEntity, TKey> 
    : ICrudService<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    private readonly DbSet<TEntity> _dataSet;
    private readonly ApplicationDbContext _dbContext;

    public EfCrudService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dataSet = _dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> GetEntities()
    {
        return _dataSet.AsQueryable();
    }

    public virtual async Task<ServiceResult> CreateAsync(TEntity entity)
    {
        try
        {
            _dataSet.Add(entity);
            await _dbContext.SaveChangesAsync();
            return ServiceResults.Success();
        }
        catch (Exception ex)
        {
            return ServiceResults.Fail(new ErrorMessage(ErrorCodes.Exception, ex.Message),
                                       new ErrorMessage(ErrorCodes.Exception, ex.InnerException?.Message ?? "No inner exception"));
        }
    }

    public virtual async Task<ServiceResult> DeleteManyAsync(IEnumerable<TKey> keys)
    {
        try
        {
            await _dataSet.Where(x => keys.Contains(x.Id))
                .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();

            return ServiceResults.Success();
        }
        catch(Exception ex)
        {
            return ServiceResults.Fail(new ErrorMessage(ErrorCodes.Exception, ex.Message));
        }
    }

    public virtual async Task<ServiceResult> DeleteAsync(TEntity entity)
    {
        try
        {
            _dataSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            
            return ServiceResults.Success();
        }
        catch (Exception ex)
        {
            return ServiceResults.Fail(new ErrorMessage(ErrorCodes.Exception, ex.Message));
        }
    }

    public virtual async Task<ServiceResult> UpdateAsync(TEntity entity)
    {
        try
        {
            _dataSet.Update(entity);
            await _dbContext.SaveChangesAsync();

            return ServiceResults.Success();
        }
        catch (Exception ex)
        {
            return ServiceResults.Fail(new ErrorMessage(ErrorCodes.Exception, ex.Message));
        }
    }

    public virtual async Task<ObjectServiceResult<TEntity>> GetByIdAsync(TKey id)
    {
        var entity = await _dataSet
            .FirstOrDefaultAsync(x => x.Id!.Equals(id));

        if (entity == null)
        {
            return ServiceResults.Fail<TEntity>(new ErrorMessage(ErrorCodes.NotFound, 
                $"{typeof(TEntity).Name} not found"));
        }

        return ServiceResults.Success(entity);
    }

}
