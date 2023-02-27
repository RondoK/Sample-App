using FastApi.EF.Tests.Data;
using FastApi.EF.Tests.Fixtures;

namespace FastApi.EF.Tests;

public class BaseEfTest
{
    protected readonly ISeededDbContext Fixture;

    public BaseEfTest(ISeededDbContext fixture)
    {
        Fixture = fixture;
    }

    protected Context GetContext()
    {
        return Fixture.GetContext();
    }
}