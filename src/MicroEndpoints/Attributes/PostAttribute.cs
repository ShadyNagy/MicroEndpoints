using MicroEndpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

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

  internal static PostAttribute GetAttributeFromMethod(MethodInfo method)
  {
    return method.GetCustomAttribute<PostAttribute>();
  }

}