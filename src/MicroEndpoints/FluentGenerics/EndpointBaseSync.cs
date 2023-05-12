using Microsoft.AspNetCore.Http;

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

    public abstract class WithIResult : EndpointConfigurationSyncBase
    {
      public abstract IResult Handle(TRequest request);
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

    public abstract class WithIResult : EndpointConfigurationSyncBase
    {
      public abstract IResult Handle();
    }
  }
}
