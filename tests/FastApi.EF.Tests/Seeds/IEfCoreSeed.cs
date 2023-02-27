using FastApi.EF.Tests.Data;
using FastApi.EF.Tests.Models;

namespace FastApi.EF.Tests.Seeds;

public interface IEfCoreSeed : IEfCoreSeed<Context>
{
    public Agg[] Aggs { get; }
}