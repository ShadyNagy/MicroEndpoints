using Microsoft.AspNetCore.Mvc;

namespace MicroEndpoints.FluentGenerics;

public static class EndpointBaseAsync
{
  public static class WithRequest<TRequest>
  {
    public abstract class WithResult<TResponse> : EndpointConfigurationAsyncBase
    {
      public abstract Task<TResponse> HandleAsync(
          TRequest request,
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithoutResult : EndpointConfigurationAsyncBase
    {
      public abstract Task HandleAsync(
          TRequest request,
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithActionResult<TResponse> : EndpointConfigurationAsyncBase
    {
      public abstract Task<ActionResult<TResponse>> HandleAsync(
          TRequest request,
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithActionResult : EndpointConfigurationAsyncBase
    {
      public abstract Task<ActionResult> HandleAsync(
          TRequest request,
          CancellationToken cancellationToken = default
      );
    }
    public abstract class WithAsyncEnumerableResult<T> : EndpointConfigurationAsyncBase
    {
      public abstract IAsyncEnumerable<T> HandleAsync(
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
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithoutResult : EndpointConfigurationAsyncBase
    {
      public abstract Task HandleAsync(
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithActionResult<TResponse> : EndpointConfigurationAsyncBase
    {
      public abstract Task<ActionResult<TResponse>> HandleAsync(
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithActionResult : EndpointConfigurationAsyncBase
    {
      public abstract Task<ActionResult> HandleAsync(
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithAsyncEnumerableResult<T> : EndpointConfigurationAsyncBase
    {
      public abstract IAsyncEnumerable<T> HandleAsync(
        CancellationToken cancellationToken = default
      );
    }
  }
}
