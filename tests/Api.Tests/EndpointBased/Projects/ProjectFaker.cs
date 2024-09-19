using App.Data.Models;
using Bogus;

namespace Api.Tests.EndpointBased.Projects;

public sealed class ProjectFaker : Faker<Project>
{
    public ProjectFaker()
    {
        RuleFor(x => x.Title, (f, p) => f.Name.FullName() + p.Id);
        RuleFor(x => x.Active, true);
        //TODO : Figure out setting of generic objects 
        //TODO : set other data
    }
}