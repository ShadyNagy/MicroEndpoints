using System.Runtime.CompilerServices;
using AutoMapper;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class Stream : EndpointBaseAsync
    .WithoutRequest
    .WithAsyncEnumerableResult<AuthorListResult>
{
  private readonly IAsyncRepository<Author> repository;
  private readonly IMapper mapper;

  public Stream(
      IAsyncRepository<Author> repository,
      IMapper mapper)
  {
    this.repository = repository;
    this.mapper = mapper;
  }

  /// <summary>
  /// Stream all authors with a one second delay between entries
  /// </summary>
  [HttpGet("api/[namespace]/stream")]
  public override async IAsyncEnumerable<AuthorListResult> HandleAsync([EnumeratorCancellation] CancellationToken cancellationToken)
  {
    var result = await repository.ListAllAsync(cancellationToken);
    foreach (var author in result)
    {
      yield return mapper.Map<AuthorListResult>(author);
      await Task.Delay(1000, cancellationToken);
    }
  }
}
