using AutoMapper;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;
using MicroEndpoints.Attributes;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class UpdateById : EndpointBaseAsync
    .WithRequest<UpdateAuthorCommandById>
    .WithActionResult<UpdatedAuthorByIdResult>
{
  private readonly IAsyncRepository<Author> _repository;
  private readonly IMapper _mapper;

  public UpdateById(IAsyncRepository<Author> repository,
      IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  /// <summary>
  /// Updates an existing Author
  /// </summary>
  [HttpPut("api/[namespace]/{id}")]
  public override async Task<ActionResult<UpdatedAuthorByIdResult>> HandleAsync([FromMultiSource]UpdateAuthorCommandById request,
    CancellationToken cancellationToken)
  {
    var author = await _repository.GetByIdAsync(request.Id, cancellationToken);

    if (author is null) return NotFound();

    author.Name = request.Details.Name;
    author.TwitterAlias = request.Details.TwitterAlias;

    await _repository.UpdateAsync(author, cancellationToken);

    var result = _mapper.Map<UpdatedAuthorByIdResult>(author);
    return result;
  }
}
