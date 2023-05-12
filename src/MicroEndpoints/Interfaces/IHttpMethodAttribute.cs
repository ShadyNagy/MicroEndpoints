using Microsoft.AspNetCore.Builder;

namespace MicroEndpoints.Interfaces;

internal interface IHttpMethodAttribute
{
  string Template { get; }
  void ConfigureEndpoint(WebApplication app, Delegate requestDelegate);
}