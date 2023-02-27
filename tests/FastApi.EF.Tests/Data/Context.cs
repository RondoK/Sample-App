using FastApi.EF.Tests.Models;
using Microsoft.EntityFrameworkCore;

namespace FastApi.EF.Tests.Data;

public class Context : DbContext
{
    public DbSet<Agg> Aggs { get; set; } = null!;

    public Context(DbContextOptions options) : base(options)
    {
    }
}