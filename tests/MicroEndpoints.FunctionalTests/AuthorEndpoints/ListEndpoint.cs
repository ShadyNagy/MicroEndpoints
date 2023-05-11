using Ardalis.HttpClientTestExtensions;
using MicroEndpoints.EndpointApp;
using MicroEndpoints.EndpointApp.DataAccess;
using MicroEndpoints.EndpointApp.DomainModel;
using MicroEndpoints.FunctionalTests.Models;

namespace MicroEndpoints.FunctionalTests.AuthorEndpoints;

public class ListEndpoint : IClassFixture<CustomWebApplicationFactory<App>>
{
  private readonly HttpClient _client;

  public ListEndpoint(CustomWebApplicationFactory<App> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsTwoGivenTwoAuthors()
  {
    var result = await _client.GetAndDeserialize<IEnumerable<Author>>(Routes.Authors.List());

    Assert.NotNull(result);
    Assert.Equal(SeedData.Authors().Count, result.Count());
  }

  [Fact]
  public async Task GivenLongRunningListRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
  {
    // Arrange, generate a token source that times out instantly
    var tokenSource = new CancellationTokenSource(TimeSpan.Zero);

    // Act
    var request = _client.GetAsync(Routes.Authors.List(), tokenSource.Token);

    // Assert
    var response = await Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
  }
}
