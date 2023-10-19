using FastApi.EF.Tests.Data;
using FastApi.EF.Tests.Models;

namespace FastApi.EF.Tests.Seeds;

public class EmptySeed : IEfCoreSeed
{
    public Agg[] Aggs { get; }

    public EmptySeed()
    {
        Aggs = Array.Empty<Agg>();
    }

    public Task Seed(Context context)
    {
        return Task.CompletedTask;
    }

    public async Task Reset(Context context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}