using Api.Tests.EndpointBased.Projects;
using App.Data;
using App.Data.Models;

namespace Api.Tests.Fixtures;

public class Seeder
{
    public readonly ProjectFaker ProjectFaker;
    public List<Project> Projects { get; }

    public Seeder()
    {
        ProjectFaker = new ProjectFaker();
        Projects = ProjectFaker.Generate(100).ToList();
    }

    public void SeedContext(Context db)
    {
        db.Projects.AddRange(Projects);
        db.SaveChanges();
    }
}