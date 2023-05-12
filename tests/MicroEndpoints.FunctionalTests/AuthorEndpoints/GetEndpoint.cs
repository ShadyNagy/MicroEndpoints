using Ardalis.HttpClientTestExtensions;
using MicroEndpoints.FunctionalTests.Models;
using MicroEndpoints.EndpointApp;
using MicroEndpoints.EndpointApp.DataAccess;
using MicroEndpoints.EndpointApp.Endpoints.Authors;
using Newtonsoft.Json;

namespace MicroEndpoints.FunctionalTests.AuthorEndpoints;

public class GetEndpoint : IClassFixture<CustomWebApplicationFactory<App>>
{
  private readonly HttpClient _client;

  public GetEndpoint(CustomWebApplicationFactory<App> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsAuthorById()
  {
    var firstAuthor = SeedData.Authors().First();

    var response = await _client.GetAsync(Routes.Authors.Get(firstAuthor.Id));
    response.EnsureSuccessStatusCode();
    var stringResponse = await response.Content.ReadAsStringAsync();

		var result = JsonConvert.DeserializeObject<AuthorResult>(stringResponse);

		Assert.NotNull(result);
    Assert.Equal(firstAuthor.Id.ToString(), result.Id);
    Assert.Equal(firstAuthor.Name, result.Name);
    Assert.Equal(firstAuthor.PluralsightUrl, result.PluralsightUrl);
    Assert.Equal(firstAuthor.TwitterAlias, result.TwitterAlias);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenInvalidAuthorId()
  {
    int invalidId = 9999;

    var response = await _client.GetAsync(Routes.Authors.Get(invalidId));
    var stringResponse = await response.Content.ReadAsStringAsync();

    response.EnsureNotFound();
	}

  [Fact]
  public async Task GivenLongRunningGetRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
  {
    // Arrange, generate a token source that times out instantly
    var tokenSource = new CancellationTokenSource(TimeSpan.Zero);
    var firstAuthor = SeedData.Authors().First();

    // Act
    var request = _client.GetAsync(Routes.Authors.Get(firstAuthor.Id), tokenSource.Token);

    // Assert
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
  }
}
