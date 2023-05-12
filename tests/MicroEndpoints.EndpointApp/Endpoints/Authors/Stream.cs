using System.Runtime.CompilerServices;
using AutoMapper;
using MicroEndpoints.Attributes;
using MicroEndpoints.FluentGenerics;
using Microsoft.AspNetCore.Mvc;
using MicroEndpoints.EndpointApp.DomainModel;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class Stream : EndpointBaseAsync
    .WithoutRequest
    .WithAsyncEnumerableResult<AuthorListResult>
{
  private IAsyncRepository<Author> _repository;
  private IMapper _mapper;

  public Stream(IAsyncRepository<Author> repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  /// <summary>
  /// Stream all authors with a one second delay between entries
  /// </summary>
  [Get("api/authors/stream")]
  public override async IAsyncEnumerable<AuthorListResult> HandleAsync([FromServices] IServiceProvider serviceProvider, [EnumeratorCancellation] CancellationToken cancellationToken = default)
  {
	  _repository = serviceProvider.GetService<IAsyncRepository<Author>>()!;
	  _mapper = serviceProvider.GetService<IMapper>()!;

		var result = await _repository.ListAllAsync(cancellationToken);
    foreach (var author in result)
    {
      yield return _mapper.Map<AuthorListResult>(author);
      await Task.Delay(1000, cancellationToken);
    }
  }
}
