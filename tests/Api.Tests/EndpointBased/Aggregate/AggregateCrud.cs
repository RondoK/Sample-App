using System.Net.Http.Json;
using Api.Tests.Fixtures;
using App.Data.Models;
using FluentAssertions;
using Xunit;

namespace Api.Tests.EndpointBased.Aggregate;

public class AggregateCrud : ResetDbFixture, IClassFixture<ClientFixture>
{
    private readonly ClientFixture _server;
    private HttpClient Api => _server.Api;
    private const string Url = Paths.Aggs;

    public AggregateCrud(ApiWebApplicationFactory factory, ClientFixture server) : base(factory)
    {
        _server = server;
    }

    [Fact]
    public async Task AddNew_ReturnsSameObjWithNewId()
    {
        var fromClient = ValidNewAggRequest();
        var createdAgg = await CreateAgg(fromClient);

        createdAgg.Should().NotBeNull();
        createdAgg.Id.Should().NotBe(fromClient.Id);
        createdAgg.Should().BeEquivalentTo(fromClient, o => o.Excluding(agg => agg.Id).ComparingByMembers<Agg>());
    }

    [Fact]
    public async Task AddNew_CanBeRetrievedById()
    {
        var created = await CreateAgg(ValidNewAggRequest());
        var retrieved = await Api.GetFromJsonAsync<Agg>(Url + $"/{created.Id}");

        retrieved.Should().BeEquivalentTo(created);
    }

    [Fact]
    public async Task AddNew_CanBeRetrievedInList()
    {
        var created = await CreateAgg(ValidNewAggRequest());
        var retrieved = await Api.GetFromJsonAsync<Agg[]>(Url + "/all");

        retrieved.Should().Contain(created);
    }

    [Fact]
    public async Task AddNew_CanBeRetrievedInPaged()
    {
        const int pageSize = 5;
        var created = await CreateAgg(ValidNewAggRequest());
        int neededPage = (created.Id / pageSize) + 1;
        var retrieved = await Api.GetFromJsonAsync<Agg[]>(Url + $"?page={neededPage}&pageSize={pageSize}");

        retrieved.Should().NotBeEmpty();
        retrieved.Should().Contain(created);
    }

    private async Task<Agg> CreateAgg(Agg request)
    {
        var addResponse = await Api.PostAsJsonAsync(Url, request);
        if (!addResponse.IsSuccessStatusCode)
            throw new Exception(
                $"Failed to create an item, response has status code {addResponse.StatusCode}.\nAnd body: " +
                await addResponse.Content.ReadAsStringAsync());
        var result = await addResponse.Content.ReadFromJsonAsync<Agg>();
        if (result == null)
            throw new Exception("Failed to create an item, response can't be casted");
        return result;
    }


    private static Agg ValidNewAggRequest()
    {
        return new Agg()
        {
            Text = "SomeText"
        };
    }
}