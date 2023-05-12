using MicroEndpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace MicroEndpoints.Attributes;

public class PutAttribute : Attribute, IHttpMethodAttribute
{
  public string Template { get; }

  public PutAttribute(string template)
  {
    Template = template;
  }

  public void ConfigureEndpoint(WebApplication app, Delegate requestDelegate)
  {
    app.MapPut(Template, requestDelegate);
  }
}