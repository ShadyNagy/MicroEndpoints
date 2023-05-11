using MicroEndpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace MicroEndpoints.Attributes;

public class GetAttribute : Attribute, IHttpMethodAttribute
{
  public string Template { get; }

  public GetAttribute(string template)
  {
    Template = template;
  }

  public void ConfigureEndpoint(WebApplication app, Delegate requestDelegate)
  {
    app.MapGet(Template, requestDelegate);
  }

  internal static GetAttribute GetAttributeFromMethod(MethodInfo method)
  {
    return method.GetCustomAttribute<GetAttribute>();
  }
}