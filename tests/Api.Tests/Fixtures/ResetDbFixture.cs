using Microsoft.Data.Sqlite;
using Xunit;

namespace Api.Tests.Fixtures;

[Collection("Sequential")]
public class ResetDbFixture: IDisposable
{
    private readonly ApiWebApplicationFactory _factory;
    //private readonly Checkpoint _checkpoint = new Checkpoint
    //{
    //    SchemasToInclude = new[] {
    //    "Playground"
    //},
    //    WithReseed = true
    //};
    
    public ResetDbFixture(ApiWebApplicationFactory factory)
    {
        _factory = factory;
        ResetDb();
        // if needed, reset the DB
        //_checkpoint.Reset(_factory.Configuration.GetConnectionString("SQL")).Wait();
    }

    private void ResetDb()
    {
        using var context = _factory.GetScopedContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
    }
}