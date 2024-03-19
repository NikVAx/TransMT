using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Abstractions;
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
