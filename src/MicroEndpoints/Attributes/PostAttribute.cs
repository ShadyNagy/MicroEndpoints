using MicroEndpoints.Interfaces;
using Microsoft.AspNetCore.Builder;

namespace MicroEndpoints.Attributes;

public class PostAttribute : Attribute, IHttpMethodAttribute
{
  public string Template { get; }

  public PostAttribute(string template)
  {
    Template = template;
  }

  public void ConfigureEndpoint(WebApplication app, Delegate requestDelegate)
  {
    app.MapPost(Template, requestDelegate);
  }
}