using Api.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.Tests.StateBased;

public class AdminAuthenticated : AuthenticatedFixture
{
    public AdminAuthenticated(ApiWebApplicationFactory factory) : base(factory, Roles.Admin)
    {
    }

    [Fact]
    public async Task AttributeProtectedResource_Ok()
    {
        var result = await Api.GetAsync(Paths.Admin.AttributeProtected);
        await result.ShouldBeOk();
        (await result.Content.ReadAsStringAsync()).Should().Be("ok");
    }
    
    [Fact]
    public async Task FluentApiProtectedResource_Ok()
    {
        var result = await Api.GetAsync(Paths.Admin.FluentApiProtected);
        
        await result.ShouldBeOk();
        (await result.Content.ReadAsStringAsync()).Should().Be("ok");
    }
}