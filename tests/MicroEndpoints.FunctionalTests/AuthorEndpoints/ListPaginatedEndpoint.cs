using Ardalis.HttpClientTestExtensions;
using MicroEndpoints.EndpointApp;
using MicroEndpoints.EndpointApp.DomainModel;
using MicroEndpoints.FunctionalTests.Models;

namespace MicroEndpoints.FunctionalTests.AuthorEndpoints;

public class ListPaginatedEndpoint : IClassFixture<CustomWebApplicationFactory<App>>
{
  private readonly HttpClient _client;

  public ListPaginatedEndpoint(CustomWebApplicationFactory<App> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task Page1PerPage1_ShouldReturnFirstAuthor()
  {
    var result = await _client.GetAndDeserialize<IEnumerable<Author>>(Routes.Authors.List(1, 1));

    Assert.NotNull(result);
    Assert.Single(result);
  }

  [Fact]
  public async Task GivenLongRunningPaginatedListRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
  {
    // Arrange, generate a token source that times out instantly
    var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(0));

    // Act
    var request = _client.GetAsync(Routes.Authors.List(1, 1), tokenSource.Token);

    // Assert
    var response = await Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
  }
}
