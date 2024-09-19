using Api.Tests.Fixtures;
using App.Data.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Api.Tests.EndpointBased.Projects;

public class ProjectReads : ResetDbFixture, IClassFixture<ClientFixture>
{
    private readonly ClientFixture _fixture;
    private readonly EndpointsGroup<Project> _server;

    public ProjectReads(ClientFixture fixture, ApiWebApplicationFactory factory, ITestOutputHelper helper) : base(factory, helper)
    {
        _fixture = fixture;
        _server = fixture.GetDefaultEndpoints<Project>(Paths.Project);
    }

    [Fact]
    public async Task RetrievedById()
    {
        var target = Seeder.Projects[Seeder.Projects.Count / 2];
        await ReadonlyTests.GetByIdAndCompare(_server, target);
    }

    [Fact]
    public async Task GetAll()
    {
        var all = await _server.GetAll();
        all.Should().BeEquivalentTo(Seeder.Projects);
    }

    [Fact]
    public async Task GetPaged()
    {
        var perPage = 10;
        var target = Seeder.Projects[Seeder.Projects.Count / 2];
        // Client code starts from 1
        var pageNo = (target.Id / perPage) + 1;
        //TODO : 
        var page = Seeder.Projects.Skip((pageNo - 1) * perPage).Take(perPage);
        var retrieved = await _server.GetPaged(pageNo, perPage);
        retrieved.Should().BeEquivalentTo(page);
    }
}