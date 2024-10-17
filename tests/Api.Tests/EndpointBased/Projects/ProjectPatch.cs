using Api.Tests.Fixtures;
using App.Data.Models;
using FluentAssertions;
using FluentAssertions.Equivalency;
using SystemTextJsonPatch;
using Xunit;
using Xunit.Abstractions;

namespace Api.Tests.EndpointBased.Projects;

public class ProjectPatch : ResetDbFixture, IClassFixture<ClientFixture>
{
    private readonly ClientFixture _fixture;
    private readonly EndpointsGroup<Project> _server;
    private readonly Project _fromClient;

    public ProjectPatch(ClientFixture fixture, ApiWebApplicationFactory factory, ITestOutputHelper helper) : base(factory, helper)
    {
        _fixture = fixture;
        _server = fixture.GetDefaultEndpoints<Project>(Paths.Project);

        _fromClient = Seeder.ProjectFaker.Generate(1).First();
    }

    [Fact]
    public async Task ProjectPatched()
    {
        // Too bulky, 
        var newElement = Seeder.ProjectFaker.Generate(1).First();

        var newTitle = newElement.Title + " updated";
        var updatePatch = new JsonPatchDocument<Project>();
        updatePatch.Replace(a => a.Title, newTitle);

        var excludeChangedProperties = new EquivalencyAssertionOptions<Project>();
        excludeChangedProperties.Excluding(x => x.Title);

        Action<Project> checkPatchedProperties = (patch) => patch.Title.Should().Be(newTitle);
        Action<Project, Project> comparePatchedResponseAndCreated =
            (patched, created) => patched.Should().BeEquivalentTo(created,
                x => x.Excluding(a => a.Title));

        await TestPreset.Patch(
            newElement,
            _server,
            updatePatch,
            checkPatchedProperties,
            comparePatchedResponseAndCreated
        );
    }
}