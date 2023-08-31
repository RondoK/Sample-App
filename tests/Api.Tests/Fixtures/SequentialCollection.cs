using Xunit;

namespace Api.Tests.Fixtures;

[CollectionDefinition("Sequential")]
public class SingleServerCollection : ICollectionFixture<ApiWebApplicationFactory> {}