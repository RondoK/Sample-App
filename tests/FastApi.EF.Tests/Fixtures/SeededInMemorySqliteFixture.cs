using FastApi.EF.Tests.Seeds;
using Xunit.Sdk;

namespace FastApi.EF.Tests.Fixtures;

public class SeededInMemorySqliteFixture<T> : InMemorySqliteFixture, ISeededDbContext
    where T : IEfCoreSeed
{
    public IEfCoreSeed Seed { get; }

    public SeededInMemorySqliteFixture()
    {
        var type = typeof(T);
        var dbSeed = typeof(DbSeed);
        Seed = type == dbSeed ? new DbSeed() : throw new NotEmptyException();
        using var context = GetContext();
        Seed.Seed(context).Wait();
    }
}