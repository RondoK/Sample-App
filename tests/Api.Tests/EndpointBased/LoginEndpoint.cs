using Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.Tests.EndpointBased;

public class LoginEndpoint : NoCleaningFixture, IClassFixture<ClientFixture>
{
    private readonly ClientFixture _server;
    public LoginEndpoint(ClientFixture server)
    {
        _server = server;
    }

    [Fact]
    public async Task ReturnsCookie()
    {
        var result = await _server.Login();
        await result.ShouldBeOk();
        result.CookieHeaders().Should().NotBeEmpty("Expected cookie response");
    }
}