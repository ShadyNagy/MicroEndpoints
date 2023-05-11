using MicroEndpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroEndpoints.Extensions;

public static class WebApplicationExtensions
{
  public static WebApplication UseMicroEndpoints(this WebApplication app)
  {
    using (var scope = app.Services.CreateScope())
    {
      var endpointConfigurations = scope.ServiceProvider.GetServices<IEndpointConfiguration>();

      foreach (var endpointConfiguration in endpointConfigurations)
      {
        endpointConfiguration.ConfigureEndpoint(app);
      }
    }

    return app;
  }
}