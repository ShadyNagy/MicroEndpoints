using AutoMapper;
using MicroEndpoints.Attributes;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class List : EndpointBaseAsync
    .WithRequest<AuthorListRequest>
    .WithResult<IEnumerable<AuthorListResult>>
{
  private readonly IAsyncRepository<Author> _repository;
  private readonly IMapper _mapper;

  public List(
      IAsyncRepository<Author> repository,
      IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  /// <summary>
  /// List all Authors
  /// </summary>
  [HttpGet("api/authors")]
  public override async Task<IEnumerable<AuthorListResult>> HandleAsync(
      [FromQuery] AuthorListRequest request,
      CancellationToken cancellationToken = default)
  {
    if (request.PerPage == 0)
    {
      request.PerPage = 10;
    }
    if (request.Page == 0)
    {
      request.Page = 1;
    }
    var result = (await _repository.ListAllAsync(request.PerPage, request.Page, cancellationToken))
        .Select(i => _mapper.Map<AuthorListResult>(i));

    return result;
  }
}
