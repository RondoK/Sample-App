using App.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit;

namespace Api.Tests.Fixtures;

//xUnit needs this class to be public
// ReSharper disable once ClassNeverInstantiated.Global
public class ApiWebApplicationFactory : WebApplicationFactory<TestProgram>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithUsername("postgres")
        .WithPassword("mysecretpassword")
        .WithDatabase("the_app")
        .Build();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json")
            .AddInMemoryCollection(new Dictionary<string, string?>()
            {
                { "ConnectionStrings:Postgres", _postgres.GetConnectionString() }
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

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgres.StopAsync();
    }
}