using Xunit;

namespace Api.Tests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class Authenticated<T> : ClientFixture, IAsyncLifetime where T : struct, IRole
{
    public Authenticated(ApiWebApplicationFactory factory) : base(factory)
    {
    }

    public async Task InitializeAsync()
    {
        await Api.MockedLogin(default(T).AsConstString());
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}