using Newtonsoft.Json;
using MicroEndpoints.FunctionalTests.Models;
using MicroEndpoints.EndpointApp;
using Ardalis.HttpClientTestExtensions;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.FunctionalTests.AuthorEndpoints;

public class DeleteEndpoint : IClassFixture<CustomWebApplicationFactory<App>>
{
  private readonly HttpClient _client;

  public DeleteEndpoint(CustomWebApplicationFactory<App> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task DeleteAnExistingAuthor()
  {
    int existingAuthorId = 2;
    string route = Routes.Authors.Delete(existingAuthorId);

    var response = await _client.DeleteAsync(route);
    response.EnsureSuccessStatusCode();

    var listResponse = await _client.GetAsync(Routes.Authors.List());
    listResponse.EnsureSuccessStatusCode();
    var stringListResponse = await listResponse.Content.ReadAsStringAsync();
    var listResult = JsonConvert.DeserializeObject<IEnumerable<Author>>(stringListResponse);

    Assert.True(listResult.Count() <= 2);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenNonexistingAuthor()
  {
    int nonexistingAuthorId = 2222;
    string route = Routes.Authors.Delete(nonexistingAuthorId);

    await _client.DeleteAndEnsureNotFound(route);
  }

  [Fact]
  public Task GivenLongRunningDeleteRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
  {
    // Arrange, generate a token source that times out instantly
    var tokenSource = new CancellationTokenSource(TimeSpan.Zero);

    // Act
    int existingAuthorId = 2;
    string route = Routes.Authors.Delete(existingAuthorId);
    var request = _client.DeleteAsync(route, tokenSource.Token);

    // Assert
    return Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
  }
}
