using Api.Tests.Fixtures;
using App.Data.Models;
using Xunit;
using Xunit.Abstractions;

namespace Api.Tests.EndpointBased.Projects;

//TODO : think about how to Run once check multiple asserts, and if it is needed
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
    [Fact]
    
    public async Task ReturnsSameObjWithNewId() =>
        await CustomAsserts.AddNew_ReturnsSameObjWithNewId<Project, int>(_fromClient, _server);

    [Fact]
    public async Task CanBeRetrievedById() =>
        await CustomAsserts.AddNew_CanBeRetrievedById<Project, int>(_fromClient, _server);
    
    [Fact]
    public async Task CanBerRetrievedInPages() =>
        await CustomAsserts.AddNew_CanBeRetrievedInPaged(_fromClient, _server, 10);

    [Fact]
    public async Task CanBeRetrievedInList() =>
        await CustomAsserts.AddNew_CanBeRetrievedInList(_fromClient, _server);
}