using System.Diagnostics.CodeAnalysis;
using Api.Tests.Fixtures;
using App.Data.Models;
using FluentAssertions;
using Xunit;

namespace Api.Tests.EndpointBased.Aggregate;

[SuppressMessage("Usage",
    "xUnit1033:Test classes decorated with \'Xunit.IClassFixture<TFixture>\' or \'Xunit.ICollectionFixture<TFixture>\' should add a constructor argument of type TFixture")]
public class AggregateCrud : ResetDbFixture, IClassFixture<ClientFixture>
{
    private readonly ClientFixture _fixture;
    private readonly EndpointsGroup<Agg> _server;

    public AggregateCrud(ClientFixture fixture, ApiWebApplicationFactory factory) : base(factory)
    {
        _fixture = fixture;
        _server = fixture.GetDefaultEndpoints<Agg>(Paths.Aggs);
    }

    [Fact]
    public async Task AddNew_ReturnsSameObjWithNewId()
    {
        var fromClient = ValidNewAggRequest();
        var createdAgg = await _server.Create(fromClient);

        createdAgg.Should().NotBeNull();
        createdAgg.Id.Should().NotBe(fromClient.Id);
        createdAgg.Should().BeEquivalentTo(fromClient, o => o.Excluding(agg => agg.Id)
            .ComparingByMembers<Agg>());
    }

    [Fact]
    public async Task AddNew_CanBeRetrievedById()
    {
        var created = await _server.Create(ValidNewAggRequest());
        var retrieved = await _server.GetById(created.Id);

        retrieved.Should().BeEquivalentTo(created);
    }

    [Fact]
    public async Task AddNew_CanBeRetrievedInList()
    {
        var created = await _server.Create(ValidNewAggRequest());
        var retrieved = await _server.GetAll();

        retrieved.Should().Contain(created);
    }

    [Fact]
    public async Task AddNew_CanBeRetrievedInPaged()
    {
        const int pageSize = 5;
        var created = await _server.Create(ValidNewAggRequest());
        var page = (created.Id / pageSize) + 1;
        var retrieved = await _server.GetPaged(page, pageSize);
        retrieved.Should().NotBeEmpty();
        retrieved.Should().Contain(created);
    }

    [Fact]
    public async Task Update()
    {
        var toCreate = ValidNewAggRequest();
        var toUpdate = await _server.Create(toCreate);
        toUpdate.Text += " CHANGED";

        var updatedResponse = await _server.Update(toUpdate);
        var loaded = await _server.GetById(toUpdate.Id);

        updatedResponse.Should().BeEquivalentTo(toUpdate);
        loaded.Should().BeEquivalentTo(updatedResponse);
        
        updatedResponse.Text.Should().NotBe(toCreate.Text);
    }

    private static Agg ValidNewAggRequest()
    {
        return new Agg()
        {
            Text = "SomeText"
        };
    }
}