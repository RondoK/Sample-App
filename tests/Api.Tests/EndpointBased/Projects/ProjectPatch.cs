using Api.Tests.Fixtures;
using App.Data.Models;
using FluentAssertions;
using FluentAssertions.Equivalency;
using SystemTextJsonPatch;
using Xunit;
using Xunit.Priority;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Api.Tests.EndpointBased.Projects;

public class ProjectPatchFixture : OneServerPerClassFixture
{
    public Project BeforePatch { get; set; }
    public Project AfterPatch { get; set; }
}

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class ProjectPatch : IClassFixture<ProjectPatchFixture>
{
    private readonly ProjectPatchFixture _fixture;
    private readonly EndpointsGroup<Project> _server;

    public ProjectPatch(ProjectPatchFixture fixture)
    {
        _fixture = fixture;
        _server = fixture.LoggedInClient.GetDefaultEndpoints<Project>(Paths.Project);
    }

    [Fact, Priority(1)]
    public async Task ProjectPatch_ResponsePatched()
    {
        var projects = _fixture.Seeder.Projects;
        _fixture.BeforePatch = projects[projects.Count / 2];
        
        var newTitle = _fixture.BeforePatch.Title + " updated";
        var updatePatch = new JsonPatchDocument<Project>();
        updatePatch.Replace(a => a.Title, newTitle);

        var excludeChangedProperties = new EquivalencyAssertionOptions<Project>();
        excludeChangedProperties.Excluding(x => x.Title);
        
        var patchedResponse = await _server.Patch(_fixture.BeforePatch.Id, updatePatch);
        patchedResponse.Title.Should().Be(newTitle);
        patchedResponse.Should().BeEquivalentTo(_fixture.BeforePatch, x => x.Excluding(x => x.Title));

        _fixture.AfterPatch = patchedResponse;
    }

    [Fact, Priority(2)]
    public async Task GetPatchedById_IsPatched()
    {
        var loaded = await _server.GetById(_fixture.BeforePatch.Id);
        loaded.Should().BeEquivalentTo(_fixture.AfterPatch);
    }
}