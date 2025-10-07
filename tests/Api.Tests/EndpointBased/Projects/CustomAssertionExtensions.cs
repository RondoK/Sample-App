using App.Data.Models;
using FluentAssertions;

namespace Api.Tests.EndpointBased.Projects;

public static class CustomAssertionExtensions
{
    // Better naming is welcome
    public static void AddNewAsserts<T, TId>(this T? actualValue, T expectedValue)
        where T : BaseEntity
    {
        actualValue.Should().NotBeNull();
        actualValue!.Id.Should().NotBe(expectedValue.Id, "Id expected to be ignored and assigned by db");
        //TODO : use same for other base properties
        // ....

        //TODO : figure out how exclude multiple keys
        actualValue.Should().BeEquivalentTo(expectedValue,
            o => o.Excluding(x => x.Id).ComparingByMembers<T>());
    }
}