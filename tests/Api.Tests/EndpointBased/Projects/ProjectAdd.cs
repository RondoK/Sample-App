using Api.Tests.Fixtures;
using App.Data.Models;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Api.Tests.EndpointBased.Projects;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class ProjectAdd : ResetDbFixture, IClassFixture<ClientFixture>
{
    private readonly ClientFixture _fixture;
    private readonly EndpointsGroup<Project> _server;
    private readonly Project _fromClient;

    public ProjectAdd(ClientFixture fixture, ApiWebApplicationFactory factory, ITestOutputHelper helper) : base(factory, helper)
    {
        _fixture = fixture;
        _server = fixture.GetDefaultEndpoints<Project>(Paths.Project);

        _fromClient = Seeder.ProjectFaker.Generate(1).First();
    }

    //TODO: Try Add AAA styled Code generation  or unwrapping function tool 
    [Fact, Priority(1)]
    public async Task ReturnsSameObjWithNewId() =>
        await TestPreset.AddNew_ReturnsSameObjWithNewId<Project, int>(_fromClient, _server);

    [Fact, Priority(2)]
    public async Task CanBeRetrievedById() =>
        await TestPreset.AddNew_CanBeRetrievedById<Project, int>(_fromClient, _server);
    
    [Fact, Priority(3)]
    public async Task CanBerRetrievedInPages() =>
        await TestPreset.AddNew_CanBeRetrievedInPaged(_fromClient, _server, 10);

    [Fact, Priority(4)]
    public async Task CanBeRetrievedInList() =>
        await TestPreset.AddNew_CanBeRetrievedInList(_fromClient, _server);
}