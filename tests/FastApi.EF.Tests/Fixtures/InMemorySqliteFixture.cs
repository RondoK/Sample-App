using System.Data.Common;
using FastApi.EF.Tests.Data;
using Microsoft.EntityFrameworkCore;

namespace FastApi.EF.Tests.Fixtures;

public class InMemorySqliteFixture : IDbContextFixture
{
    private static int _dbNo;
    private static readonly object LockObj = new();
    //private const string _connectionString = "DataSource=file::memory:?cache=shared";
    private readonly DbContextOptions _options;
    private readonly DbConnection _connection;

    public Context GetContext()
    {
        return new Context(_options);
    }

    protected InMemorySqliteFixture()
    {
        string connectionString;
        lock (LockObj)
        {
            connectionString = $"DataSource=file:{_dbNo++}?mode=memory&cache=shared";
        }
            
        _options = new DbContextOptionsBuilder<Context>()
            .UseSqlite(connectionString, c => c.MigrationsAssembly("App.Data.Sqlite"))
            .Options;
        using var context = GetContext();
        _connection = context.Database.GetDbConnection();
        _connection.Open();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}