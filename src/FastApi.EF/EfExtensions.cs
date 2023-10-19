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
    /// <summary>
    /// </summary>
    /// <param name="query"></param>
    /// <param name="page"></param>
    /// <param name="pageSize">Starts from 1</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> Paged<T>(this IQueryable<T> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    //TODO: Probably, it should return paged object with extra info, besides list by itself {page, pageSize, maxPages ... etc.}
    /// <summary>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="page"></param>
    /// <param name="pageSize">Starts from 1</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<List<T>> GetPageAsync<T>(this DbContext source, int page, int pageSize)
        where T : class
    {
        return source.Set<T>().Paged(page, pageSize).ToListAsync();
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

    public static T Update<T>(this DbContext source, T obj)
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
            .FirstOrDefaultAsync(filter, token);
    }

    public static T? FindNoTracking<T>(this DbContext source, params object[] keyValues)
        where T : class
    {
        var filter = source.FirstWithKeysExpression<T>(keyValues);
        return source.Set<T>().AsNoTracking()
            .FirstOrDefault(filter);
    }

    public static Task<T?> FindNoTrackingAsync<T>(this DbContext source, params object[] keyValues)
        where T : class
    {
        var filter = source.FirstWithKeysExpression<T>(keyValues);
        return source.Set<T>().AsNoTracking()
            .FirstOrDefaultAsync(filter);
    }

    public static Expression<Func<T, bool>> FirstWithKeysExpression<T>(this DbContext source,
        params object[] keyValues)
        where T : class
    {
        if (keyValues is not { Length: > 0 })
        {
            throw new Exception("No Keys Provided.");
        }

        var keyProps = GetKeyProperties<T>(source);
        if (keyProps.Length != keyValues.Length)
        {
            throw new Exception("Incorrect Number of Keys Provided.");
        }

        ParameterExpression prm = Expression.Parameter(typeof(T));
        Expression body = Expression.Constant(true);
        for (int i = 0; i < keyProps.Length; i++)
        {
            PropertyInfo pi = keyProps[i]!;
            object value = keyValues[i];
            Expression propertyEx = Expression.Property(prm, pi);
            Expression valueEx = Expression.Constant(value);
            Expression condition = Expression.Equal(propertyEx, valueEx);
            body = Expression.AndAlso(body, condition);
        }


        //TODO : think about caching the value without specific keys
        var filter = Expression.Lambda<Func<T, bool>>(body, prm);
        return filter;
    }

    public static PropertyInfo?[] GetKeyProperties<T>(this DbContext source)
    {
        //TODO : think about caching the value, also caching(or strongly-typed key access) should solve potential multiple NRE here
        //TODO: check out how model keys search is implemented in Odata
        return source.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.Select(p => p.PropertyInfo).ToArray() ?? 
               throw new Exception("Can't find key parameters for " + typeof(T));
    }
}