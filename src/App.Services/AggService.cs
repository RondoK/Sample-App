using App.Data;
using App.Data.Models;
using FastApi.EF;
using Microsoft.EntityFrameworkCore;

namespace App.Services;

public class AggService(Context db) : IProvidePaged<Agg>
{
    public Task<List<Agg>> GetPageAsync(int page, int pageSize)
    {
        return db.Aggs
            .OrderBy(x => x.Id)
            .Paged(page, pageSize)
            .ToListAsync();
    }
}