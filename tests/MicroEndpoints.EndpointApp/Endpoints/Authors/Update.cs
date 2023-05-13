using AutoMapper;
using MicroEndpoints.Attributes;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class Update : EndpointBaseAsync
    .WithRequest<UpdateAuthorCommand>
    .WithIResult
{
  private IAsyncRepository<Author> _repository;
  private IMapper _mapper;

  public Update(IAsyncRepository<Author> repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  /// <summary>
  /// Updates an existing Author
  /// </summary>
  [Put("api/authors")]
  public override async Task<IResult> HandleAsync([FromServices] IServiceProvider serviceProvider, [FromBody] UpdateAuthorCommand request, CancellationToken cancellationToken = default)
  {
	  _repository = serviceProvider.GetService<IAsyncRepository<Author>>()!;
	  _mapper = serviceProvider.GetService<IMapper>()!;

		var author = await _repository.GetByIdAsync(request.Id, cancellationToken);

    if (author is null) return NotFound();

    _mapper.Map(request, author);
    await _repository.UpdateAsync(author, cancellationToken);

    var result = _mapper.Map<UpdatedAuthorResult>(author);
    return Ok(result);
  }
}
