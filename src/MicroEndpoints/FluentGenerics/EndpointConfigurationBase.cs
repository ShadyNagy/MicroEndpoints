using System;
using System.Linq.Expressions;
using System.Reflection;
using MicroEndpoints.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroEndpoints.FluentGenerics;

[ApiExplorerSettings(IgnoreApi = true)]
public abstract class EndpointConfigurationBase : IEndpointConfiguration
{
  protected abstract string HandleName { get; }

  public void ConfigureEndpoint(WebApplication app)
  {
	  var method = GetType().GetMethod(HandleName);
	  if (method == null)
	  {
		  throw new Exception($"No method named {HandleName} found in {GetType().Name}");
	  }

	  var httpMethodAttributes = method.GetCustomAttributes().OfType<IHttpMethodAttribute>();

	  foreach (var httpMethodAttribute in httpMethodAttributes)
	  {
		  var requestDelegate = CreateRequestDelegate();
		  httpMethodAttribute.ConfigureEndpoint(app, requestDelegate);
	  }
  }

  public static IResult Ok(object? result = null)
  {
	  return Results.Ok(result);
  }

  public static IResult Created(string uri, object? result = null)
  {
	  return Results.Created(uri, result);
  }

  public static IResult NoContent()
	{
	  return Results.NoContent();
	}

  public static IResult BadRequest(object? error=null)
  {
	  return Results.BadRequest(error);
  }

  public static IResult Unauthorized()
  {
	  return Results.Unauthorized();
  }

  public static IResult Forbidden(AuthenticationProperties? properties = null, IList<string>? authenticationSchemes = null)
  {
	  return Results.Forbid(properties, authenticationSchemes);
  }

  public static IResult NotFound(object? value=null)
  {
	  return Results.NotFound(value);
  }

  public static IResult File(byte[] fileContents, string contentType, string fileDownloadName)
  {
	  return Results.File(fileContents, contentType, fileDownloadName);
  }

  public static IResult CreatedAtRoute(string routeName, object routeValues, object value)
  {
	  return Results.CreatedAtRoute(routeName, routeValues, value);
  }

  public static IResult Accepted(string? uri = null, object? value = null)
  {
	  return Results.Accepted(uri, value);
  }

  public static IResult AcceptedAtRoute(string? routeName = null, object? routeValues = null, object? value = null)
  {
	  return Results.AcceptedAtRoute(routeName, routeValues, value);
  }

	private static IResult Problem(string? detail = null,
	  string? instance = null,
	  int? statusCode = null,
	  string? title = null,
	  string? type = null,
	  IDictionary<string, object?>? extensions = null)
  {
	  return Results.Problem(detail, instance, statusCode, title, type, extensions);
  }

	protected virtual Delegate CreateRequestDelegate()
  {
	  var method = GetType().GetMethod(HandleName);
	  if (method == null)
		  throw new Exception($"No method named {HandleName} found in {GetType().Name}");

	  var delegateType = CreateDelegateType(method);

	  var handleAsyncDelegate = method.CreateDelegate(delegateType, this);

	  return handleAsyncDelegate;
  }

  private Type CreateDelegateType(MethodInfo methodInfo)
  {
	  var types = methodInfo.GetParameters().Select(p => p.ParameterType).ToList();

	  if (methodInfo.ReturnType != typeof(void))
		  types.Add(methodInfo.ReturnType);

	  return Expression.GetDelegateType(types.ToArray());
  }
}