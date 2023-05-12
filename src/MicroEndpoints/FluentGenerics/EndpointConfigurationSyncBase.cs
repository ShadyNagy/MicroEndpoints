using Microsoft.AspNetCore.Mvc;

namespace MicroEndpoints.FluentGenerics;

[ApiExplorerSettings(IgnoreApi = true)]
public abstract class EndpointConfigurationSyncBase : EndpointConfigurationBase
{
  protected override string HandleName => "Handle";
}