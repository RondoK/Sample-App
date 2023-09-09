using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.Data;

public class DefaultDesignTimeFactory : IDesignTimeDbContextFactory<Context>
{
    private readonly string _defaultConnectionString;
    private readonly IEfContextParamsFactory _contextParamsFactory;

    public Context CreateDbContext(string[] args)
    {
        var connectionString = MigrationParamsParser.GetConnectionStringOrDefault(args, _defaultConnectionString);

        return CreateDbContext(connectionString);
    }

    private Context CreateDbContext(string connectionString)
    {
        _contextParamsFactory.SetConnectionString(connectionString);
        var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
        _contextParamsFactory.BuildOptionsDelegate().Invoke(optionsBuilder);

        return new Context(optionsBuilder.Options, _contextParamsFactory.CreateModelCreatingOptions());
    }

    public DefaultDesignTimeFactory(string defaultConnectionString, IEfContextParamsFactory contextParamsFactory)
    {
        _defaultConnectionString = defaultConnectionString;
        _contextParamsFactory = contextParamsFactory;
    }
}