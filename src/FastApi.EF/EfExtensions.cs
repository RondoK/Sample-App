using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace FastApi.EF;

/*
 We can use static methods directly in the MinimalApiEndpoints or use
 Some sort of IService with BaseService : IService
 And SpecificAggService : BaseService
 stt
*/
public static class EfExtensions
{
    /// <param name="page">Starts from 1</param>
    public static IQueryable<T> Paged<T>(this IQueryable<T> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    //TODO: Probably, it should return paged object with extra info, besides list by itself {page, pageSize, maxPages ... etc.}
    /// <param name="page">Starts from 1</param>
    public static Task<List<T>> GetPageAsync<T>(this DbContext source, int page, int pageSize)
        where T : class
    {
        return source.Set<T>().Paged(page, pageSize).ToListAsync();
        ;
    }

    public static Task<List<T>> GetAll<T>(this DbContext source)
        where T : class
    {
        return source.Set<T>().ToListAsync();
    }

    public static void Delete<T>(this DbContext source, T obj)
        where T : class
    {
        source.Set<T>().Remove(obj);
        source.SaveChanges();
    }

    public static async Task DeleteAsync<T>(this DbContext source, T obj)
        where T : class
    {
        source.Set<T>().Remove(obj);
        await source.SaveChangesAsync();
    }

    public static async Task<T> SaveUpdateAsync<T>(this DbContext source, T obj, CancellationToken token = default)
        where T : class
    {
        source.Set<T>().Update(obj);
        await source.SaveChangesAsync(token);
        return obj;
    }

    public static T? Update<T>(this DbContext source, T obj)
        where T : class
    {
        source.Set<T>().Update(obj);
        source.SaveChanges();
        return obj;
    }

    public static async Task<T?> SaveNewAsync<T>(this DbContext source, T obj, CancellationToken token = default)
        where T : class
    {
        source.Set<T>().Add(obj);
        await source.SaveChangesAsync(token);
        return obj;
    }

    public static Task<T?> FindNoTrackingAsync<T>(this DbContext source, CancellationToken token = default,
        params object[] keyValues)
        where T : class
    {
        var filter = source.FirstWithKeysExpression<T>(keyValues);
        return source.Set<T>().AsNoTracking()
            .FirstOrDefaultAsync(filter!, token);
    }

    public static T? FindNoTracking<T>(this DbContext source, params object[] keyValues)
        where T : class
    {
        var filter = source.FirstWithKeysExpression<T>(keyValues);
        return source.Set<T>().AsNoTracking()
            .FirstOrDefault(filter!);
    }

    public static Task<T?> FindNoTrackingAsync<T>(this DbContext source, params object[] keyValues)
        where T : class
    {
        var filter = source.FirstWithKeysExpression<T>(keyValues);
        return source.Set<T>().AsNoTracking()
            .FirstOrDefaultAsync(filter!);
    }

    public static Expression<Func<T, bool>?> FirstWithKeysExpression<T>(this DbContext source,
        params object[] keyValues)
        where T : class
    {
        if (keyValues == null || !keyValues.Any())
        {
            throw new Exception("No Keys Provided.");
        }

        PropertyInfo[] keyProps = GetKeyProperties<T>(source);
        if (keyProps.Count() != keyValues.Count())
        {
            throw new Exception("Incorrect Number of Keys Provided.");
        }

        ParameterExpression prm = Expression.Parameter(typeof(T));
        Expression body = null;
        for (int i = 0; i < keyProps.Count(); i++)
        {
            PropertyInfo pi = keyProps[i];
            object value = keyValues[i];
            Expression propertyEx = Expression.Property(prm, pi);
            Expression valueEx = Expression.Constant(value);
            Expression condition = Expression.Equal(propertyEx, valueEx);
            body = body == null ? condition : Expression.AndAlso(body, condition);
        }


        //TODO : think about caching the value without specific keys
        var filter = Expression.Lambda<Func<T, bool>>(body, prm);
        return filter;
    }

    public static PropertyInfo[] GetKeyProperties<T>(this DbContext source)
    {
        //TODO : think about caching the value
        return source.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(p => p.PropertyInfo).ToArray();
    }
}