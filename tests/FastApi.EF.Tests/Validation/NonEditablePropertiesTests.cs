using FastApi.EF.Tests.Models;
using FastApi.Endpoints;
using FluentAssertions;
using SystemTextJsonPatch;

namespace FastApi.EF.Tests.Validation;

public class NonEditableTests
{
    [Fact]
    public void ForbiddenPropertyChanged_Invalid ()
    {
        var validation = new NonEditableProperties(Config.Create<Agg>(a => a.Text!));

        var patch = new JsonPatchDocument<Agg>();
        patch.Replace(a => a.Text, "New text");
        
        validation.IsValidPatch(patch).Should().BeFalse();
    }

    [Fact]
    public void OneOfChangedPropertiesForbidden_Invalid()
    {
        var validation = new NonEditableProperties(Config.Create<Agg>(a => a.Text!));

        var patch = new JsonPatchDocument<Agg>();
        patch.Replace(a => a.Id, 1);
        patch.Replace(a => a.Text, "New text");
        
        validation.IsValidPatch(patch).Should().BeFalse();
    }

    [Fact]
    public void NotForbiddenPropertyChanged_Valid()
    {
        var validation = new NonEditableProperties(Config.Create<Agg>(a => a.Text!));

        var patch = new JsonPatchDocument<Agg>();
        patch.Replace(a => a.Id, 1);
        
        validation.IsValidPatch(patch).Should().BeTrue();
    }
}