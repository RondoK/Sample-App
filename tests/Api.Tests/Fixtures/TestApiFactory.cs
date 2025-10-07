using App.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Api.Tests.Fixtures;

public class TestApiFactory(PostgreSqlContainer db) : WebApplicationFactory<TestProgram>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json")
            .AddInMemoryCollection(new Dictionary<string, string?>()
            {
                { "ConnectionStrings:Postgres", db.GetConnectionString() }
            })
            .Build();
        builder.UseConfiguration(configuration);
    }

    public IServiceScope CreateScope()
    {
        return Services.CreateScope();
    }

    public Context GetScopedContext()
    {
        return CreateScope().ServiceProvider.GetService<Context>()!;
    }
}