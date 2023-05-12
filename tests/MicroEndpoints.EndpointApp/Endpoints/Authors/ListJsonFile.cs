using System.Text.Json;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class ListJsonFile : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
  private readonly IAsyncRepository<Author> repository;

  public ListJsonFile(IAsyncRepository<Author> repository)
  {
    this.repository = repository;
  }

  /// <summary>
  /// List all Authors as a JSON file
  /// </summary>
  [HttpGet("api/authors/Json")]
  public override async Task<ActionResult> HandleAsync(
      CancellationToken cancellationToken = default)
  {
    var result = (await repository.ListAllAsync(cancellationToken)).ToList();

    var streamData = JsonSerializer.SerializeToUtf8Bytes(result);
    return File(streamData, "text/json", "authors.json");
  }
}
