using AutoMapper;
using MicroEndpoints.Attributes;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class Create : EndpointBaseAsync
    .WithRequest<CreateAuthorCommand>
    .WithActionResult
{
  private readonly IAsyncRepository<Author> _repository;
  private readonly IMapper _mapper;

  public Create(IAsyncRepository<Author> repository,
      IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  [Post("api/authors")]
  public override async Task<ActionResult> HandleAsync([FromBody] CreateAuthorCommand request, CancellationToken cancellationToken)
  {
    var author = new Author();
    _mapper.Map(request, author);
    await _repository.AddAsync(author, cancellationToken);

    var result = _mapper.Map<CreateAuthorResult>(author);
    return CreatedAtRoute("Authors_Get", new { id = result.Id }, result);
  }
}
