using App.Data.Postgres.Configurations;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Postgres;

public class ConfigurePostgres : IConfigureModelCreating
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AggConfiguration).Assembly);
    }
}