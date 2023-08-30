using Api;
using Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.Tests.TestEndpoints;

public class MockedLoginTestFixture : ResetDbFixture
{
    public MockedLoginTestFixture(ApiWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ReturnsACookie()
    {
        var roles = new[] { Roles.Admin };
        var result = await Api.MockedLogin(roles);
        result.CookieHeaders().Should().NotBeEmpty();
        //TODO: decrypt and check roles list
    }
}