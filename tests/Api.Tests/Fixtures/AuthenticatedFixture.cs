using Xunit;

namespace Api.Tests.Fixtures;

public class AuthenticatedFixture : ResetDbFixture, IAsyncLifetime
{
    private readonly string _authenticatedRole;
    public AuthenticatedFixture(ApiWebApplicationFactory factory, string authenticatedRole) : base(factory)
    {
        _authenticatedRole = authenticatedRole;
    }

    public async Task InitializeAsync()
    {
        await Api.MockedLogin(_authenticatedRole);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}