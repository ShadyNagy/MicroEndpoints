using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroEndpoints.Models;

public class MicroResponses
{
  public static IActionResult Ok(object result = null)
  {
    return new OkObjectResult(result);
  }

  public static IActionResult Created(string uri, object result = null)
  {
    return new CreatedResult(uri, result);
  }

  public static IActionResult NoContent()
  {
    return new NoContentResult();
  }

  public static IActionResult BadRequest(string message = "Bad request")
  {
    return ProblemDetailsResult(StatusCodes.Status400BadRequest, "Bad Request", message);
  }

  public static IActionResult Unauthorized(string message = "Unauthorized")
  {
    return ProblemDetailsResult(StatusCodes.Status401Unauthorized, "Unauthorized", message);
  }

  public static IActionResult Forbidden(string message = "Forbidden")
  {
    return ProblemDetailsResult(StatusCodes.Status403Forbidden, "Forbidden", message);
  }

  public static IActionResult NotFound(string message = "Resource not found")
  {
    return ProblemDetailsResult(StatusCodes.Status404NotFound, "Not Found", message);
  }

  public static IActionResult InternalServerError(string message = "Internal server error")
  {
    return ProblemDetailsResult(StatusCodes.Status500InternalServerError, "Internal Server Error", message);
  }

  private static IActionResult ProblemDetailsResult(int statusCode, string title, string detail)
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
}