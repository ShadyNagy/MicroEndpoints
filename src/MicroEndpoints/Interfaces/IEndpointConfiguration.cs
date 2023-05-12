using Microsoft.AspNetCore.Builder;

namespace MicroEndpoints.Interfaces;


internal interface IEndpointConfiguration
{
  void ConfigureEndpoint(WebApplication app);
}