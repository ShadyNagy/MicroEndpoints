using System.Text.Json;
using MicroEndpoints.Attributes;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class ListJsonFile : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
  private readonly IAsyncRepository<Author> _repository;

  public ListJsonFile(IAsyncRepository<Author> repository)
  {
    _repository = repository;
  }

  /// <summary>
  /// List all Authors as a JSON file
  /// </summary>
  [Get("api/authors/Json")]
  public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
  {
    var result = (await _repository.ListAllAsync(cancellationToken)).ToList();

    var streamData = JsonSerializer.SerializeToUtf8Bytes(result);
    return File(streamData, "text/json", "authors.json");
  }
}
