using Microsoft.EntityFrameworkCore;

namespace App.Data.SqlServer;

public class ConfigureSqlServer : IConfigureModelCreating
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConfigureSqlServer).Assembly);
    }
}