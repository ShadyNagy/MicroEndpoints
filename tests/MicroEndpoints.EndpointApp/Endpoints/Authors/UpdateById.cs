using AutoMapper;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;
using MicroEndpoints.Attributes;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class UpdateById : EndpointBaseAsync
    .WithRequest<UpdateAuthorCommandById>
    .WithIResult
{
  private IAsyncRepository<Author> _repository;
  private IMapper _mapper;

  public UpdateById(IAsyncRepository<Author> repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  /// <summary>
  /// Updates an existing Author
  /// </summary>
  [Put("api/authors/{id}")]
  public override async Task<IResult> HandleAsync([FromServices] IServiceProvider serviceProvider, [FromMultiSource]UpdateAuthorCommandById request, CancellationToken cancellationToken = default)
  {
	  _repository = serviceProvider.GetService<IAsyncRepository<Author>>()!;
	  _mapper = serviceProvider.GetService<IMapper>()!;

		var author = await _repository.GetByIdAsync(request.Id, cancellationToken);

    if (author is null) return Results.NotFound();

    author.Name = request.Details.Name;
    author.TwitterAlias = request.Details.TwitterAlias;

    await _repository.UpdateAsync(author, cancellationToken);

    var result = _mapper.Map<UpdatedAuthorByIdResult>(author);
    return Results.Ok(result);
  }
}
