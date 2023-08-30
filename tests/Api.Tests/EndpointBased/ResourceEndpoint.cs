using Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.Tests.EndpointBased;

public class ResourceEndpoint: ResetDbFixture
{
    private const string Path = Paths.Resource;

    [Fact]
    public async Task Authenticated_Ok()
    {
        await Login();
        var result = await Api.GetAsync(Path);
        await result.ShouldBeOk();
        (await result.Content.ReadAsStringAsync()).Should().Be("ok");
    }

    public ResourceEndpoint(ApiWebApplicationFactory factory) : base(factory)
    {
    }
}