using Microsoft.AspNetCore.Builder;

namespace MicroEndpoints.Interfaces;

public interface IHttpMethodAttribute
{
  string Template { get; }
  void ConfigureEndpoint(WebApplication app, Delegate requestDelegate);
}