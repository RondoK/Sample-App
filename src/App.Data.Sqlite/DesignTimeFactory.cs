using App.Data.Sqlite.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Internal;

namespace App.Data.Sqlite;

/*
public class DesignTimeFactory : DbContextFactory<Context>, IConfigureModelCreating //IDesignTimeDbContextFactory<DbContext>, IConfigureModelCreating
{
    public DbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
        optionsBuilder.UseSqlite("Data Source=app.db", o => o.MigrationsAssembly("App.Data.Sqlite"));

        return new Context(optionsBuilder.Options);
    }
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DesignTimeFactory).Assembly);
    }

    public DesignTimeFactory(IServiceProvider serviceProvider, DbContextOptions<Context> options, IDbContextFactorySource<Context> factorySource) 
        : base(serviceProvider, options, factorySource)
    {
    }
}
*/


public class ConfigureSqlite : IConfigureModelCreating
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AggConfiguration).Assembly);
    }
}