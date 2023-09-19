using Microsoft.EntityFrameworkCore;

namespace App.Data.SqlServer;

public class SqlServerContextParamsFactory : IEfContextParamsFactory
{
    private string _connectionString;

    public SqlServerContextParamsFactory(string connectionString)
    {
        this._connectionString = connectionString;
    }

    public void SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Action<DbContextOptionsBuilder> BuildOptionsDelegate() => options => options.UseSqlServer(_connectionString,
        c => c.MigrationsAssembly("App.Data.SqlServer"));

    public IConfigureModelCreating CreateModelCreatingOptions() => new ConfigureSqlServer();
}