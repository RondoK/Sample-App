using Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.Tests.EndpointBased;

public class LoginEndpoint : ApiTestFixture
{
    public LoginEndpoint(ApiWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ReturnsCookie()
    {
        var result = await Login();
        await result.ShouldBeOk();
        result.CookieHeaders().Should().NotBeEmpty("Expected cookie response");
    }
}