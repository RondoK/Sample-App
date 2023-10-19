using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Tests.Fixtures;

public class ClientFixture
{
    public readonly HttpClient Api;
    
    public ClientFixture(ApiWebApplicationFactory factory)
    {
        Api = factory.CreateClient(new WebApplicationFactoryClientOptions()
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

    public async Task<HttpResponseMessage> MockLogin(params string[] roles)
    {
        return await Api.MockedLogin(roles);
    }
}