using System.Text;
using Newtonsoft.Json;
using MicroEndpoints.FunctionalTests.Models;
using MicroEndpoints.EndpointApp;
using Ardalis.HttpClientTestExtensions;
using MicroEndpoints.EndpointApp.Endpoints.Authors;
using MicroEndpoints.EndpointApp.DataAccess;

namespace MicroEndpoints.FunctionalTests.AuthorEndpoints;

public class UpdateEndpoint : IClassFixture<CustomWebApplicationFactory<App>>
{
  private readonly HttpClient _client;

  public UpdateEndpoint(CustomWebApplicationFactory<App> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task UpdatesAnExistingAuthor()
  {
    var updatedAuthor = new UpdateAuthorCommand()
    {
      Id = 2,
      Name = "James Eastham",
    };

    var authorPreUpdate = SeedData.Authors().First(p => p.Id == 2);

    var response = await _client.PutAsync(Routes.Authors.Update, new StringContent(JsonConvert.SerializeObject(updatedAuthor), Encoding.UTF8, "application/json"));

    response.EnsureSuccessStatusCode();
    var stringResponse = await response.Content.ReadAsStringAsync();
    var result = JsonConvert.DeserializeObject<UpdatedAuthorResult>(stringResponse);

    Assert.NotNull(result);
    Assert.Equal(result.Id, updatedAuthor.Id.ToString());
    Assert.NotEqual(result.Name, authorPreUpdate.Name);
    Assert.Equal("James Eastham", result.Name);
    Assert.Equal(result.PluralsightUrl, authorPreUpdate.PluralsightUrl);
    Assert.Equal(result.TwitterAlias, authorPreUpdate.TwitterAlias);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenNonexistingAuthor()
  {
    var updatedAuthor = new UpdateAuthorCommand()
    {
      Id = 2222, // invalid author
      Name = "Doesn't Matter",
    };

    await _client.PutAndEnsureNotFoundAsync(Routes.Authors.Update, new StringContent(JsonConvert.SerializeObject(updatedAuthor), Encoding.UTF8, "application/json"));
  }

  [Fact]
  public async Task GivenLongRunningUpdateRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
  {
    // Arrange, generate a token source that times out instantly
    var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(0));
    var authorPreUpdate = SeedData.Authors().FirstOrDefault(p => p.Id == 2);
    var updatedAuthor = new UpdateAuthorCommand()
    {
      Id = 2,
      Name = "James Eastham",
    };

    // Act
    var request = _client.PutAsync(Routes.Authors.Update, new StringContent(JsonConvert.SerializeObject(updatedAuthor), Encoding.UTF8, "application/json"), tokenSource.Token);

    // Assert
    await Assert.ThrowsAsync<TaskCanceledException>(async () => await request);
  }
}
