using Api.Tests.Fixtures;
using App.Data.Models;
using FluentAssertions;
using Xunit;

namespace Api.Tests.EndpointBased.Projects;

public class ProjectReads(OneServerPerClassFixture fixture) : IClassFixture<OneServerPerClassFixture>
{
    private readonly List<Project> _projects = fixture.Seeder.Projects;

    private readonly EndpointsGroup<Project> _projectEndpoints 
        = fixture.LoggedInClient.GetDefaultEndpoints<Project>(Paths.Project);
    
    [Fact]
    public async Task RetrievedById()
    {
        var target = _projects[_projects.Count / 2];
        await ReadonlyTests.GetByIdAndCompare(_projectEndpoints, target);
    }

    [Fact]
    public async Task GetAll()
    {
        var all = await _projectEndpoints.GetAll();
        all.Should().BeEquivalentTo(_projects);
    }

    [Fact]
    public async Task GetPaged()
    {
        var perPage = 10;
        var target = _projects[_projects.Count / 2];
        // Client code starts from 1
        var pageNo = (target.Id / perPage) + 1;
        //TODO : 
        var page = _projects.Skip((pageNo - 1) * perPage).Take(perPage);
        var retrieved = await _projectEndpoints.GetPaged(pageNo, perPage);
        retrieved.Should().BeEquivalentTo(page);
    }
}