using Api.Tests.Fixtures;
using App.Data.Models;
using Xunit;
using Xunit.Priority;

namespace Api.Tests.EndpointBased.Projects;
public class ProjectAddFixture : OneServerPerClassFixture
{
    public Project CreatedProject { get; set; } = null!;
}

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class ProjectAdd : IClassFixture<ProjectAddFixture>
{
    private readonly ProjectAddFixture _fixture;
    private readonly EndpointsGroup<Project> _server; 
    private Project CreatedProject => _fixture.CreatedProject;

    public ProjectAdd(ProjectAddFixture fixture)
    {
        _fixture = fixture;
        _server = fixture.LoggedInClient.GetDefaultEndpoints<Project>(Paths.Project);
    }
    
    [Fact, Priority(1)]
    public async Task Created_ReturnsSameObjWithNewId()
    {
        var fromClient = _fixture.Seeder.ProjectFaker.Generate(1).First();
        // Not sure that it looks good, but need to do extra volume testing of the tooling.
        _fixture.CreatedProject = await TestPreset.AddNew_ReturnsSameObjWithNewId<Project, int>(fromClient, _server);
    }

    [Fact, Priority(2)]
    public async Task CanBeRetrievedById() =>
        await ReadonlyTests.GetByIdAndCompare(_server, CreatedProject);
    
    [Fact, Priority(3)]
    public async Task CanBerRetrievedInPages() =>
        await ReadonlyTests.IsInPagedResponse(_server, CreatedProject);

    [Fact, Priority(4)]
    public async Task CanBeRetrievedInList() =>
        await ReadonlyTests.IsInGetAllResponse(_server, CreatedProject);
}