using MicroEndpoints.Attributes;
using MicroEndpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroEndpoints.FluentGenerics;

public abstract class EndpointConfigurationBase : IEndpointConfiguration
{
  protected abstract string HandleName { get; }

  public void ConfigureEndpoint(WebApplication app)
  {
    var httpMethodAttributes = new List<IHttpMethodAttribute>
        {
            GetAttribute.GetAttributeFromMethod(GetType().GetMethod(HandleName)!),
            PostAttribute.GetAttributeFromMethod(GetType().GetMethod(HandleName)!),
            PutAttribute.GetAttributeFromMethod(GetType().GetMethod(HandleName)!),
            DeleteAttribute.GetAttributeFromMethod(GetType().GetMethod(HandleName)!)
        };

    var requestDelegate = CreateRequestDelegate();

    foreach (var httpMethodAttribute in httpMethodAttributes)
    {
      if (httpMethodAttribute != null)
      {
        httpMethodAttribute.ConfigureEndpoint(app, requestDelegate);
      }
    }
  }

  public static ActionResult Ok(object? result = null)
  {
    return new OkObjectResult(result);
  }

  public static ActionResult Created(string uri, object? result = null)
  {
    return new CreatedResult(uri, result);
  }

  public static ActionResult NoContent()
  {
    return new NoContentResult();
  }

  public static ActionResult BadRequest(string message = "Bad request")
  {
    return ProblemDetailsResult(StatusCodes.Status400BadRequest, "Bad Request", message);
  }

  public static ActionResult Unauthorized(string message = "Unauthorized")
  {
    return ProblemDetailsResult(StatusCodes.Status401Unauthorized, "Unauthorized", message);
  }

  public static ActionResult Forbidden(string message = "Forbidden")
  {
    return ProblemDetailsResult(StatusCodes.Status403Forbidden, "Forbidden", message);
  }

  public static ActionResult NotFound(string message = "Resource not found")
  {
    return ProblemDetailsResult(StatusCodes.Status404NotFound, "Not Found", message);
  }

  public static ActionResult InternalServerError(string message = "Internal server error")
  {
    return ProblemDetailsResult(StatusCodes.Status500InternalServerError, "Internal Server Error", message);
  }

  public static ActionResult File(byte[] fileContents, string contentType, string fileDownloadName)
  {
    return new FileContentResult(fileContents, contentType)
    {
      FileDownloadName = fileDownloadName
    };
  }

  public static ActionResult CreatedAtRoute(string routeName, object routeValues, object value)
  {
    return new CreatedAtRouteResult(routeName, routeValues, value);
  }

  private static ActionResult ProblemDetailsResult(int statusCode, string title, string detail)
  {
    var problemDetails = new ProblemDetails
    {
      Status = statusCode,
      Title = title,
      Detail = detail
    };

    return new ObjectResult(problemDetails)
    {
      StatusCode = statusCode
    };
  }

  protected virtual Delegate CreateRequestDelegate()
  {
    var method = GetType().GetMethod(HandleName, new[] { typeof(CancellationToken) });
    var handleAsyncDelegate = method.CreateDelegate(typeof(Func<CancellationToken, Task>), this);
    return handleAsyncDelegate;
  }
}