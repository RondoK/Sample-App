using App.Data.Sqlite.Configurations;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Sqlite;

public class ConfigureSqlite : IConfigureModelCreating
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AggConfiguration).Assembly);
    }
}