using App.Data;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Tests.Fixtures;

//xUnit needs this class to be public
// ReSharper disable once ClassNeverInstantiated.Global
public class ApiWebApplicationFactory : WebApplicationFactory<TestProgram>, IAsyncLifetime
{
    private readonly IContainer _postgres = new ContainerBuilder()
        .WithImage("postgres:latest")
        .WithEnvironment("POSTGRES_USER", "postgres")
        .WithEnvironment("POSTGRES_PASSWORD", "mysecretpassword")
        .WithEnvironment("POSTGRES_DB", "the_app")
        .WithPortBinding(5555, 5432)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .Build();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json")
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
