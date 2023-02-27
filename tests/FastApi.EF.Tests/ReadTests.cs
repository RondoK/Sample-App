using FluentAssertions;
using FastApi.EF;
using FastApi.EF.Tests.Fixtures;
using FastApi.EF.Tests.Models;
using FastApi.EF.Tests.Seeds;

namespace FastApi.EF.Tests;

public class ReadTests : BaseEfTest, IClassFixture<SeededInMemorySqliteFixture<DbSeed>>
{
    private Agg[] PreCreatedAggs => Fixture.Seed.Aggs;
    public ReadTests(SeededInMemorySqliteFixture<DbSeed> fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task FindNoTracking()
    {
        var expected = PreCreatedAggs[PreCreatedAggs.Length / 2];

        await using var context = GetContext();
        var found = await context.FindNoTrackingAsync<Agg>(expected.Id);
        found.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetPaged()
    {
        var expected = PreCreatedAggs.Skip(5).Take(5).ToArray();

        await using var context = GetContext();
        var paged = await context.GetPageAsync<Agg>(2, 5);

        paged.Should().BeEquivalentTo(expected);
    }
}