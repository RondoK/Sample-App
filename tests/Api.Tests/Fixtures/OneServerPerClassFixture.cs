using Testcontainers.PostgreSql;
using Xunit;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Api.Tests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class OneServerPerClassFixture : IAsyncLifetime
{
    public TestApiFactory AppFactory { get; private set; }
    public HttpClient LoggedInClient { get; private set; }

    private PostgreSqlContainer _postgres;
    public Seeder Seeder { get; private set; }

    public async Task InitializeAsync()
    {
        _postgres = await ChannelHolder.DbChannel.Reader.ReadAsync();
        AppFactory = new TestApiFactory(_postgres);
        Seeder = new Seeder();
        
        await using var context = AppFactory.GetScopedContext();
        await context.Database.EnsureCreatedAsync();
        Seeder.SeedContext(context);

        LoggedInClient = AppFactory.CreateClient();
        await LoggedInClient.Login();
    }

    public async Task DisposeAsync()
    {
        await using (var context = AppFactory.GetScopedContext())
        {
            await context.Database.EnsureDeletedAsync();
        }

        await ChannelHolder.DbChannel.Writer.WriteAsync(_postgres);
    }
}