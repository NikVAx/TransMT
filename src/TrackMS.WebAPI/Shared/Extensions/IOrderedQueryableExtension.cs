namespace TrackMS.WebAPI.Shared.Extensions;

public static class IOrderedQueryableExtension
{
    public static IQueryable<TEntity> GetPage<TEntity>(this IOrderedQueryable<TEntity> entities,
        int pageSize, int pageIndex)
    {
        return entities.Skip(pageSize * pageIndex).Take(pageSize);
    }
}
