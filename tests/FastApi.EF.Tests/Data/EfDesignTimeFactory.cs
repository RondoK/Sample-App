using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FastApi.EF.Tests.Data;

public class EfDesignTimeFactory : IDesignTimeDbContextFactory<Context>
{
    /*
     * Shared flag : https://stackoverflow.com/a/56367786
     * SQLite also supports named shared in-memory databases. By using the same connection string, multiple SqliteConnection objects can connect to the same database.
     * However: the database is automatically deleted and memory is reclaimed when the last connection to the database closes.
     * So  it is still necessary to maintain a separate open connection object for the database to be usable across multiple EF Core calls. 
     */

    private const string ConnectionString = "DataSource=myshareddb;mode=memory;cache=shared";
    public Context CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<Context>()
            .UseSqlite(ConnectionString)
            .Options;

        var context = new Context(options);
        context.Database.EnsureCreated();
        return context;
    }
}