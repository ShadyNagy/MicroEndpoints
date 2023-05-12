using Microsoft.AspNetCore.Mvc;

namespace MicroEndpoints.FluentGenerics;

[ApiExplorerSettings(IgnoreApi = true)]
public abstract class EndpointConfigurationAsyncBase : EndpointConfigurationBase
{
  protected override string HandleName => "HandleAsync";
}