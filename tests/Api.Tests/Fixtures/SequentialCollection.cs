using Xunit;

namespace Api.Tests.Fixtures;

[CollectionDefinition("Sequential")]
public class SequentialCollection : ICollectionFixture<ApiWebApplicationFactory> {}