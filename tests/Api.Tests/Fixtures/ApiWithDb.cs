using App.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests.Fixtures;

public class ApiWithDb
{
    protected readonly ApiWebApplicationFactory Factory;
    protected readonly HttpClient Api;

    public ApiWithDb(ApiWebApplicationFactory factory)
    {
        Factory = factory;
        Api = Factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            //TODO : Figure out what for https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#mock-authentication
            AllowAutoRedirect = false,
            HandleCookies = true
        });
    }
    
    public async Task<HttpResponseMessage> Login()
    {
        return await Api.GetAsync(Paths.Login);
    }

    public IServiceScope CreateScope()
    {
        return Factory.Services.CreateScope();
    }

    public Context GetScopedContext()
    {
        return CreateScope().ServiceProvider.GetService<Context>()!;
    }
}