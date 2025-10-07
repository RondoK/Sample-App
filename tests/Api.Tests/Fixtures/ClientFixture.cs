using System.Diagnostics;
using Api.Tests.EndpointBased.Projects;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Api.Tests.Fixtures;

/// <summary>
/// Requires CollectionFixture with ApiWebApplicationFactory
/// </summary>
public class ClientFixture
{
    public readonly HttpClient Api;
    private readonly IMessageSink _sink;
    
    // From Collection Fixture
    public ClientFixture(ApiWebApplicationFactory factory, IMessageSink sink)
    {
        _sink = sink;
        _sink.OnMessage(new DiagnosticMessage("Client Fixture Constructor"));
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