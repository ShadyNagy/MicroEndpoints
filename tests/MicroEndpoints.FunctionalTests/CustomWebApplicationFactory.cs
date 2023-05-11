using MicroEndpoints.EndpointApp;
using MicroEndpoints.EndpointApp.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MicroEndpoints.FunctionalTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<App>
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder
        .UseSolutionRelativeContentRoot("Tests/MicroEndpoints.EndpointApp")
        .ConfigureServices(services =>
        {
          var descriptor = services.SingleOrDefault(
          d => d.ServiceType ==
             typeof(DbContextOptions<AppDbContext>));

          if (descriptor != null)
          {
            // remove default (real) implementation
            services.Remove(descriptor);
          }

          // Create a new service provider.
          var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

          // Add a database context (AppDbContext) using an in-memory
          // database for testing.
          services.AddDbContext<AppDbContext>(options =>
          {
            options.UseInMemoryDatabase($"InMemoryDbForTesting");
            options.UseInternalServiceProvider(serviceProvider);
          });

          // Build the service provider.
          var sp = services.BuildServiceProvider();

          // Create a scope to obtain a reference to the database
          // context (AppDbContext).
          using var scope = sp.CreateScope();
          var scopedServices = scope.ServiceProvider;
          var db = scopedServices.GetRequiredService<AppDbContext>();

          var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

          // Ensure the database is created.
          db.Database.EnsureCreated();

          try
          {
            // Seed the database with test data.
            SeedData.PopulateTestData(db);
          }
          catch (Exception ex)
          {
            logger.LogError(ex, "An error occurred seeding the " +
                      $"database with test messages. Error: {ex.Message}");
          }
        });
  }
}
