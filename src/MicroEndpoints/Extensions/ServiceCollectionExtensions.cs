using MicroEndpoints.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroEndpoints.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddMicroEndpoints(this IServiceCollection services, Assembly? assembly = null)
  {
    assembly = assembly ?? Assembly.GetExecutingAssembly();

    var endpointConfigurationType = typeof(IEndpointConfiguration);
    var endpointConfigurationImplementations = assembly
        .ExportedTypes
        .Where(t => endpointConfigurationType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

    foreach (var endpointConfigurationImplementation in endpointConfigurationImplementations)
    {
      services.AddScoped(endpointConfigurationType, endpointConfigurationImplementation);
    }

    return services;
  }
}