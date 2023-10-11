using Microsoft.EntityFrameworkCore;

namespace App.Data.Postgres;

public class PostgresContextParamsFactory : IEfContextParamsFactory
{
    private string _connectionString;

    public PostgresContextParamsFactory(string connectionString)
    {
        this._connectionString = connectionString;
    }

    public void SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Action<DbContextOptionsBuilder> BuildOptionsDelegate() =>
        options => options.UseNpgsql(_connectionString,
                c => c.MigrationsAssembly("App.Data.Postgres"))
            .UseSnakeCaseNamingConvention();

    public IConfigureModelCreating CreateModelCreatingOptions() => new ConfigurePostgres();
}