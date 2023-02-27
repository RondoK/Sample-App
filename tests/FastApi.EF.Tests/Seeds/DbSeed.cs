using FastApi.EF.Tests.Data;
using FastApi.EF.Tests.Models;

namespace FastApi.EF.Tests.Seeds;

public class DbSeed : IEfCoreSeed
{
    public Agg[] Aggs { get; private set; }

    private static Agg[] GetAggs()
    {
        return Enumerable.Range(1, 20)
            .Select(id => new Agg() { Text = id.ToString() })
            .ToArray();
    }

    public async Task Seed(Context context)
    {
        Aggs = GetAggs();
        context.Aggs.AddRange(Aggs);
        await context.SaveChangesAsync();
    }

    public async Task Reset(Context context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await Seed(context);
    }
}