using Api.Tests.Fixtures;
using App.Data.Models;
using Xunit;
using Xunit.Abstractions;

namespace Api.Tests.EndpointBased.Projects;

public class ProjectUpdate : ResetDbFixture, IClassFixture<ClientFixture>
{
    private readonly ClientFixture _fixture;
    private readonly EndpointsGroup<Project> _server;
    private readonly Project _fromClient;

    public ProjectUpdate(ClientFixture fixture, ApiWebApplicationFactory factory, ITestOutputHelper helper) : base(factory, helper)
    {
        helper.WriteLine("Project Update Start");
        _fixture = fixture;
        _server = fixture.GetDefaultEndpoints<Project>(Paths.Project);

        _fromClient = Seeder.ProjectFaker.Generate(1).First();
    }

    [Fact]
    public async Task FullUpdateRetrievableById()
    {
        var newElement = Seeder.ProjectFaker.Generate(1).First();
        await CustomAsserts.Update_FullUpdated(newElement,
            p => p.Title += " updated", _server);
    }
}