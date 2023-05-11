using Microsoft.AspNetCore.Mvc;

namespace MicroEndpoints.FluentGenerics;

public static class EndpointBaseSync
{
  public static class WithRequest<TRequest>
  {
    public abstract class WithResult<TResponse> : EndpointConfigurationSyncBase
    {
      public abstract TResponse Handle(TRequest request);
    }

    public abstract class WithoutResult : EndpointConfigurationSyncBase
    {
      public abstract void Handle(TRequest request);
    }

    public abstract class WithActionResult<TResponse> : EndpointConfigurationSyncBase
    {
      public abstract ActionResult<TResponse> Handle(TRequest request);
    }

    public abstract class WithActionResult : EndpointConfigurationSyncBase
    {
      public abstract ActionResult Handle(TRequest request);
    }
  }

  public static class WithoutRequest
  {
    public abstract class WithResult<TResponse> : EndpointConfigurationSyncBase
    {
      public abstract TResponse Handle();
    }

    public abstract class WithoutResult : EndpointConfigurationSyncBase
    {
      public abstract void Handle();
    }

    public abstract class WithActionResult<TResponse> : EndpointConfigurationSyncBase
    {
      public abstract ActionResult<TResponse> Handle();
    }

    public abstract class WithActionResult : EndpointConfigurationSyncBase
    {
      public abstract ActionResult Handle();
    }
  }
}
