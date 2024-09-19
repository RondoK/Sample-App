using System.Diagnostics;
using Api.Tests.EndpointBased.Projects;
using Microsoft.Data.Sqlite;
using Xunit;
using Xunit.Abstractions;

namespace Api.Tests.Fixtures;

[Collection("Sequential")]
public class ResetDbFixture : IDisposable
{
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly ApiWebApplicationFactory _factory;
    private readonly ITestOutputHelper _output;

    public Seeder Seeder;
    //private readonly Checkpoint _checkpoint = new Checkpoint
    //{
    //    SchemasToInclude = new[] {
    //    "Playground"
    //},
    //    WithReseed = true
    //};

    public ResetDbFixture(ApiWebApplicationFactory factory, ITestOutputHelper output)
    {
        _stopwatch.Start();
        _factory = factory;
        _output = output;
        Seeder = new Seeder();
        ResetDb();
        _stopwatch.Stop();
        _stopwatch.Reset();
        // if needed, reset the DB
        //_checkpoint.Reset(_factory.Configuration.GetConnectionString("SQL")).Wait();
    }

    private void ResetDb()
    {
        using var context = _factory.GetScopedContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        Seeder.SeedContext(context);
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
    }
}