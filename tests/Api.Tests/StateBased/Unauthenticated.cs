using Api.Tests.Fixtures;
using Xunit;

namespace Api.Tests.StateBased;

public class Unauthenticated: NoCleaningFixture, IClassFixture<ClientFixture>
{
    private readonly ClientFixture _server;
    public Unauthenticated(ApiWebApplicationFactory factory, ClientFixture server)
    {
        _server = server;
    }

    [Theory]
    [MemberData(nameof(PathsList))]
    public Task Route_IsRedirected(string path) => _server.Api.GetAndCheckRedirected(path);

    public static IEnumerable<object[]> PathsList => new List<object[]>
    {
        new object[] { Paths.Admin.AttributeProtected },
        new object[] { Paths.Resource },
        new object[] { Paths.Admin.FluentApiProtected },
    };
}