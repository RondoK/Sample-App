using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Tests.Fixtures;

[Trait("Category", "Integration")]
public class ApiTestFixture : IClassFixture<ApiWebApplicationFactory>, IDisposable
{
    protected readonly ApiWebApplicationFactory Factory;
    protected readonly HttpClient Api;

    //private readonly Checkpoint _checkpoint = new Checkpoint
    //{
    //    SchemasToInclude = new[] {
    //    "Playground"
    //},
    //    WithReseed = true
    //};

    public async Task<HttpResponseMessage> Login()
    {
        return await Api.GetAsync(Paths.Login);
    }

    public IServiceScope CreateScope()
    {
        return Factory.Services.CreateScope();
    }

    public ApiTestFixture(ApiWebApplicationFactory factory)
    {
        Factory = factory;
        Api = Factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            //TODO : Figure out what for https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#mock-authentication
            AllowAutoRedirect = false,
            HandleCookies = true
        });
        // if needed, reset the DB
        //_checkpoint.Reset(_factory.Configuration.GetConnectionString("SQL")).Wait();
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
    }
}