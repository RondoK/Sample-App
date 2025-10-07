using FastApi.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace FastApi.EF;

public class ProvidePaged<T, TId> : IProvidePaged<T>
    where T : class, IHaveId<TId>
{
    private readonly DbContext _db;

    public ProvidePaged(DbContext db)
    {
        _db = db;
    }

    public Task<List<T>> GetPageAsync(int page, int pageSize)
    {
        return _db.Set<T>()
            .OrderBy(x => x.Id)
            .Paged(page, pageSize)
            .ToListAsync();
    }
}