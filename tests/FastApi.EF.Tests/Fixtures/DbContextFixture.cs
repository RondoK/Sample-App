using FastApi.EF.Tests.Data;
using FastApi.EF.Tests.Seeds;

namespace FastApi.EF.Tests.Fixtures;

public interface IDbContextFixture : IDisposable
{
    Context GetContext();
}

public interface ISeededDbContext : IDbContextFixture
{
    IEfCoreSeed Seed { get; }
    
}