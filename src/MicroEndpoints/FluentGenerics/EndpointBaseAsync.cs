using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroEndpoints.FluentGenerics;

public static class EndpointBaseAsync
{
  public static class WithRequest<TRequest>
  {
    public abstract class WithResult<TResponse> : EndpointConfigurationAsyncBase
    {
      public abstract Task<TResponse> HandleAsync(
	      [FromServices] IServiceProvider serviceProvider,
	      TRequest request,
        CancellationToken cancellationToken = default
      );
    }

    public abstract class WithoutResult : EndpointConfigurationAsyncBase
    {
      public abstract Task HandleAsync(
	      [FromServices] IServiceProvider serviceProvider,
				TRequest request,
        CancellationToken cancellationToken = default
      );
    }

    public abstract class WithIResult : EndpointConfigurationAsyncBase
    {
      public abstract Task<IResult> HandleAsync(
	      [FromServices] IServiceProvider serviceProvider,
				TRequest request,
        CancellationToken cancellationToken = default
      );
    }
    public abstract class WithAsyncEnumerableResult<T> : EndpointConfigurationAsyncBase
    {
      public abstract IAsyncEnumerable<T> HandleAsync(
	      [FromServices] IServiceProvider serviceProvider,
				TRequest request,
        CancellationToken cancellationToken = default
      );
    }
  }

  public static class WithoutRequest
  {
    public abstract class WithResult<TResponse> : EndpointConfigurationAsyncBase
    {
      public abstract Task<TResponse> HandleAsync(
	      [FromServices] IServiceProvider serviceProvider,
				CancellationToken cancellationToken = default
      );
    }

    public abstract class WithoutResult : EndpointConfigurationAsyncBase
    {
      public abstract Task HandleAsync(
	      [FromServices] IServiceProvider serviceProvider,
				CancellationToken cancellationToken = default
      );
    }

    public abstract class WithIResult : EndpointConfigurationAsyncBase
    {
      public abstract Task<IResult> HandleAsync(
	      [FromServices] IServiceProvider serviceProvider,
				CancellationToken cancellationToken = default
      );
    }

    public abstract class WithAsyncEnumerableResult<T> : EndpointConfigurationAsyncBase
    {
      public abstract IAsyncEnumerable<T> HandleAsync(
	      [FromServices] IServiceProvider serviceProvider,
				CancellationToken cancellationToken = default
      );
    }
  }
}
