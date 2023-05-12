using MicroEndpoints.Attributes;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class Delete : EndpointBaseAsync
    .WithRequest<DeleteAuthorRequest>
    .WithActionResult
{
  private readonly IAsyncRepository<Author> _repository;

  public Delete(IAsyncRepository<Author> repository)
  {
    _repository = repository;
  }

  /// <summary>
  /// Deletes an Author
  /// </summary>
  [HttpDelete("api/authors/{id}")]
  public override async Task<ActionResult> HandleAsync([FromRoute] DeleteAuthorRequest request, CancellationToken cancellationToken)
  {
    var author = await _repository.GetByIdAsync(request.Id, cancellationToken);

    if (author is null)
    {
      return NotFound(request.Id.ToString());
    }

    await _repository.DeleteAsync(author, cancellationToken);

    return NoContent();
  }
}
