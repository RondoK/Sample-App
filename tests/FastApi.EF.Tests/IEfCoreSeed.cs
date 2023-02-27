using Microsoft.EntityFrameworkCore;

namespace FastApi.EF.Tests;

public interface IEfCoreSeed<in T>
    where T : DbContext
{
    public Task Seed(T context);

    public Task Reset(T context);
}