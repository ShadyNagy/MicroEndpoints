using MicroEndpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace MicroEndpoints.Attributes;

public class DeleteAttribute : Attribute, IHttpMethodAttribute
{
  public string Template { get; }

  public DeleteAttribute(string template)
  {
    Template = template;
  }

  public void ConfigureEndpoint(WebApplication app, Delegate requestDelegate)
  {
    app.MapDelete(Template, requestDelegate);
  }
}