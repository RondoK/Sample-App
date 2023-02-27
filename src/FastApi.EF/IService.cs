using Microsoft.EntityFrameworkCore;

namespace FastApi.EF;

/*
 * We can use static methods directly in the MinimalApiEndpoints
 * or use some sort of IService with BaseService : IService
 * and OverridenService : BaseService
*/

//IService can be customized for your default MinimalApi setup

/*
 * It is not IRepository, because I see it like something more than work with db
 */
public interface IService<TEntity>
{
    Task<List<TEntity>> GetAll();
    Task Delete(TEntity obj);
    Task<TEntity> SaveUpdate(TEntity obj);
    Task<TEntity?> SaveNew(TEntity obj);
    Task<TEntity?> FindNoTracking(params object[] keyValues);
    Task<List<TEntity>> GetPaged(int page, int pageSize);
}

public class GenericService<T> : IService<T> where T : class
{
    private DbContext _context;

    public GenericService(DbContext context)
    {
        _context = context;
    }

    public virtual Task<List<T>> GetAll() => _context.GetAll<T>();

    public virtual Task Delete(T obj) => _context.DeleteAsync(obj);

    public virtual Task<T> SaveUpdate(T obj) => _context.SaveUpdateAsync(obj);

    public virtual Task<T?> SaveNew(T obj) => _context.SaveNewAsync(obj);

    public virtual Task<T?> FindNoTracking(params object[] keyValues) => _context.FindNoTrackingAsync<T>(keyValues);

    public virtual Task<List<T>> GetPaged(int page, int pageSize) => _context.GetPageAsync<T>(page, pageSize);
}