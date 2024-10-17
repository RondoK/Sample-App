using Testcontainers.PostgreSql;
using Xunit;

namespace Api.Tests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class OneServerPerClassFixture : IAsyncLifetime
{
    public TestApiFactory AppFactory { get; }
    public HttpClient LoggedInClient { get; private set; }
    
    private readonly PostgreSqlContainer _postgres;
    public Seeder Seeder { get; }

    public OneServerPerClassFixture()
    {
        _postgres = new PostgreSqlBuilder()
            .WithUsername("postgres")
            .WithPassword("mysecretpassword")
            .WithDatabase("the_app")
            .Build();
        AppFactory = new TestApiFactory(_postgres);
        this.Seeder = new Seeder();
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        await using var context = AppFactory.GetScopedContext();
        await context.Database.EnsureCreatedAsync();
        this.Seeder.SeedContext(context);

        LoggedInClient = AppFactory.CreateClient();
        await LoggedInClient.Login();
    }

    public async Task DisposeAsync()
    {
        await _postgres.StopAsync();
    }
}