using Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.Tests.TestEndpoints;

public class MockedLoginTestFixture : NoCleaningFixture, IClassFixture<ClientFixture>
{
    private ClientFixture _server;
    public MockedLoginTestFixture(ApiWebApplicationFactory factory, ClientFixture server)
    {
        _server = server;
    }

    [Fact]
    public async Task ReturnsACookie()
    {
        var roles = new[] { Roles.Admin };
        var result = await _server.Api.MockedLogin(roles);
        result.CookieHeaders().Should().NotBeEmpty();
        //TODO: decrypt and check roles list
    }
}