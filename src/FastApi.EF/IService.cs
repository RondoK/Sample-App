using Microsoft.EntityFrameworkCore;

namespace FastApi.EF;

/// <summary>
/// interface for Generic service
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IService<TEntity>
{
    Task<List<TEntity>> GetAll();
    Task Delete(TEntity obj);
    Task<TEntity> SaveUpdate(TEntity obj);
    Task<TEntity?> SaveNew(TEntity obj);
    Task<TEntity?> FindNoTracking(params object[] keyValues);
    Task<List<TEntity>> GetPaged(int page, int pageSize);
}

/// <summary>
///  Generic service - wrapper over standalone extension actions of FastApi.EF
/// </summary>
/// <typeparam name="T"></typeparam>
public class GenericService<T> : IService<T> where T : class
{
    private readonly DbContext _context;

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