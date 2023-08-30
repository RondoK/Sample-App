using System.Net;
using Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.Tests.EndpointBased;

public class UserEndpoint : ResetDbFixture
{
    private const string Path = "/user";

    public UserEndpoint(ApiWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ReturnsOk()
    {
        var result = await Api.GetAsync(Path);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}