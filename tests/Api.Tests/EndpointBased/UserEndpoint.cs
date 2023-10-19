using System.Net;
using Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.Tests.EndpointBased;

public class UserEndpoint : NoCleaningFixture, IClassFixture<ClientFixture>
{
    private readonly ClientFixture _server;
    private const string Path = "/user";

    public UserEndpoint(ClientFixture server)
    {
        _server = server;
    }

    [Fact]
    public async Task ReturnsOk()
    {
        var result = await _server.Api.GetAsync(Path);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}