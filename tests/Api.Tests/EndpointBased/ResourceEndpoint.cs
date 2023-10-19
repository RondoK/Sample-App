using Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.Tests.EndpointBased;

public class ResourceEndpoint: NoCleaningFixture, IClassFixture<ClientFixture>
{
    private const string Path = Paths.Resource;
    private readonly ClientFixture _server;

    [Fact]
    public async Task Authenticated_Ok()
    {
        await _server.Login();
        var result = await _server.Api.GetAsync(Path);
        await result.ShouldBeOk();
        (await result.Content.ReadAsStringAsync()).Should().Be("ok");
    }

    public ResourceEndpoint(ClientFixture server)
    {
        _server = server;
    }
}