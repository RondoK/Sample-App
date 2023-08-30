using Api.Tests.Fixtures;
using Xunit;

namespace Api.Tests.StateBased;

public class Unauthenticated : ResetDbFixture
{
    public Unauthenticated(ApiWebApplicationFactory factory) : base(factory)
    {
    }

    [Theory]
    [MemberData(nameof(PathsList))]
    public Task Route_IsRedirected(string path) => Api.GetAndCheckRedirected(path);

    public static IEnumerable<object[]> PathsList => new List<object[]>
    {
        new object[] { Paths.Admin.AttributeProtected },
        new object[] { Paths.Resource },
        new object[] { Paths.Admin.FluentApiProtected },
    };
}