using Api.Tests.Fixtures;
using App.Data.Models;
using FluentAssertions;
using Xunit;
using Xunit.Priority;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Api.Tests.EndpointBased.Projects;

public class ProjectUpdateFixture : OneServerPerClassFixture
{
    public Project AfterUpdate { get; set; }
}

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class ProjectUpdate : IClassFixture<ProjectUpdateFixture>
{
    private readonly ProjectUpdateFixture _fixture;
    private readonly EndpointsGroup<Project> _server;

    public ProjectUpdate(ProjectUpdateFixture fixture)
    {
        _fixture = fixture;
        _server = fixture.LoggedInClient.GetDefaultEndpoints<Project>(Paths.Project);
    }

    [Fact, Priority(1)]
    public async Task FullUpdate_ReturnsUpdated()
    {
        var projects = _fixture.Seeder.Projects;
        var beforeUpdate = projects[projects.Count / 2];
        
        var fromClient = beforeUpdate.ShallowClone<Project>();
        fromClient.Title = beforeUpdate.Title + " updated";
        
        var updatedResponse = await _server.Update(fromClient);

        updatedResponse.Should().BeEquivalentTo(fromClient);

        _fixture.AfterUpdate = updatedResponse;
    }

    [Fact, Priority(2)]
    public async Task GetById_ReturnsUpdated()
    {
        var loaded = await _server.GetById(_fixture.AfterUpdate.Id);

        loaded.Should().BeEquivalentTo(_fixture.AfterUpdate);
    }
}