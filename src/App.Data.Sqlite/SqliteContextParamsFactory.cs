using Microsoft.EntityFrameworkCore;

namespace App.Data.Sqlite;

public class SqliteContextParamsFactory : IEfContextParamsFactory
{
    private string _connectionString;

    public SqliteContextParamsFactory(string connectionString)
    {
        this._connectionString = connectionString;
    }

    public void SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Action<DbContextOptionsBuilder> BuildOptionsDelegate() => options => options.UseSqlite(_connectionString,
        c => c.MigrationsAssembly("App.Data.Sqlite"));

    public IConfigureModelCreating CreateModelCreatingOptions() => new ConfigureSqlite();
}