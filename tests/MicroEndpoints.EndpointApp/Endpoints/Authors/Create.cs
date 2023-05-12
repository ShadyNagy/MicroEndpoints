using AutoMapper;
using MicroEndpoints.Attributes;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class Create : EndpointBaseAsync
    .WithRequest<CreateAuthorCommand>
    .WithIResult
{
  private IAsyncRepository<Author> _repository;
  private IMapper _mapper;

  public Create(IAsyncRepository<Author> repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  [Post("api/authors")]
  public override async Task<IResult> HandleAsync([FromServices] IServiceProvider serviceProvider, [FromBody] CreateAuthorCommand request, CancellationToken cancellationToken = default)
  {
	  _repository = serviceProvider.GetService<IAsyncRepository<Author>>()!;
	  _mapper = serviceProvider.GetService<IMapper>()!;

		var author = new Author();
    _mapper.Map(request, author);
    await _repository.AddAsync(author, cancellationToken);

    var result = _mapper.Map<CreateAuthorResult>(author);
    return Results.CreatedAtRoute("authors_get", new { id = result.Id }, result);
  }
}
