using App.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Data;

public class Context : DbContext
{
    private readonly IConfigureModelCreating _configure;
    public DbSet<Agg> Aggs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _configure.OnModelCreating(modelBuilder);
    }

    public Context(DbContextOptions options, IConfigureModelCreating configure) : base(options)
    {
        _configure = configure;
    }
}