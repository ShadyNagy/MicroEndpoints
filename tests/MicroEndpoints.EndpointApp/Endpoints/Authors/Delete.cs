using MicroEndpoints.Attributes;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class Delete : EndpointBaseAsync
    .WithRequest<int>
    .WithIResult
{
  private IAsyncRepository<Author> _repository;

  public Delete(IAsyncRepository<Author> repository)
  {
    _repository = repository;
  }

  /// <summary>
  /// Deletes an Author
  /// </summary>
  [Delete("api/authors/{id:int}")]
  public override async Task<IResult> HandleAsync([FromServices] IServiceProvider serviceProvider, [FromRoute] int id, CancellationToken cancellationToken = default)
  {
	  _repository = serviceProvider.GetService<IAsyncRepository<Author>>()!;

	  var author = await _repository.GetByIdAsync(id, cancellationToken);

    if (author is null)
    {
      return NotFound(id.ToString());
    }

    await _repository.DeleteAsync(author, cancellationToken);

    return NoContent();
  }
}
