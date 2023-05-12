[![publish to nuget](https://github.com/ShadyNagy/MicroEndpoints/actions/workflows/nugt-publish.yml/badge.svg)](https://github.com/ShadyNagy/MicroEndpoints/actions/workflows/nugt-publish.yml)
[![MicroEndpoints on NuGet](https://img.shields.io/nuget/v/MicroEndpoints?label=MicroEndpoints)](https://www.nuget.org/packages/MicroEndpoints/)
[![NuGet](https://img.shields.io/nuget/dt/MicroEndpoints)](https://www.nuget.org/packages/MicroEndpoints)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/ShadyNagy/MicroEndpoints/blob/main/LICENSE)
[![paypal](https://img.shields.io/badge/PayPal-tip%20me-green.svg?logo=paypal)](https://www.paypal.me/shadynagy)


# MicroEndpoints

MicroEndpoints is a .NET library that simplifies creating HTTP endpoints in a minimalistic and clean manner. It's designed to work with .NET 6's Minimal APIs. 

Minimal APIs in .NET 6 offer an easy way to build HTTP APIs with the flexibility to add more features as your project grows. They are great for creating HTTP APIs where you typically send and receive HTTP requests and responses. These APIs are often used with single-page apps, mobile clients, and for public HTTP APIs.

Here are a few benefits of Minimal APIs:

- Simplicity: With Minimal APIs, you can use a single line of code to express an API endpoint, making it easy to understand.
- Productivity: Minimal APIs require less code and less ceremony, which leads to a productivity gain.
- Performance: Minimal APIs are built on top of the same high-performance components as the rest of ASP.NET Core.

For more detailed information, please refer to the [Microsoft Documentation on Minimal APIs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0).

With MicroEndpoints, you can leverage the simplicity and performance of Minimal APIs with a structured and easy-to-use model, making your development process even more efficient.


## Getting Started

To install the library, you can use the .NET CLI:

```shell
dotnet add package MicroEndpoints
```

After installing the package, make sure to include the following line in your `Program.cs`:

```csharp
//...
builder.Services.AddMicroEndpoints(Assembly.GetAssembly(typeof(Program)));
//...
var app = builder.Build();
//...
app.UseMicroEndpoints();

app.Run();
```

# Endpoint Types

MicroEndpoints provides the flexibility to create synchronous or asynchronous endpoints based on your application's needs. 

## Synchronous Endpoints

If you don't have any I/O-bound work that would benefit from asynchrony, you can create synchronous endpoints. Here's an example:

```csharp
public class MySyncEndpoint : EndpointBaseSync
    .WithRequest<MyRequest>
    .WithIResult
{
  public override IResult Handle(MyRequest request)
  {
    // Implementation...
  }
}
```

In the example above, `MySyncEndpoint` is a synchronous endpoint that handles a `MyRequest` request and returns an `IResult`.

## Asynchronous Endpoints

If you're performing I/O-bound operations, such as network requests or database queries, you should use asynchronous endpoints. Here's an example:

```csharp
public class MyAsyncEndpoint : EndpointBaseAsync
    .WithRequest<MyRequest>
    .WithIResult
{
  public override async Task<IResult> HandleAsync(MyRequest request, CancellationToken cancellationToken = default)
  {
    // Implementation...
  }
}
```

In this example, `MyAsyncEndpoint` is an asynchronous endpoint that handles a `MyRequest` request and returns an `IResult`.

# Endpoint Results

The result of an endpoint can be of any type. However, it's recommended to use `IResult` for its flexibility and expressiveness. `IResult` allows you to easily produce different HTTP responses.

If you want to return a custom type, you can do so by specifying the type when inheriting from `EndpointBaseAsync` or `EndpointBaseSync`. Here's an example:

```csharp
public class MyEndpoint : EndpointBaseAsync
    .WithRequest<MyRequest>
    .WithResult<MyResponse> // MyResponse is a custom class
{
  public override async Task<MyResponse> HandleAsync(MyRequest request, CancellationToken cancellationToken = default)
  {
    // Implementation...
  }
}
```

In the example above, `MyEndpoint` is an asynchronous endpoint that handles a `MyRequest` request and returns a `MyResponse` result.

# Dependency Injection

If you need to use services from the dependency injection (DI) container in your endpoints, you can do so within the `Handle` or `HandleAsync` methods. 

You can use the `GetService` method from the `IServiceProvider` to retrieve your services. Here's an example:

```csharp
public override async Task<IResult> HandleAsync([FromServices] IServiceProvider serviceProvider, MyRequest request, CancellationToken cancellationToken = default)
{
  // Get services from the DI container
  _repository = serviceProvider.GetService<IAsyncRepository<Author>>()!;
  _mapper = serviceProvider.GetService<IMapper>()!;

  // Implementation...
}
```

In the example above, `IAsyncRepository<Author>` and `IMapper` are being retrieved from the service provider, which is injected into the method by the framework.

Remember to add a reference to the `Microsoft.Extensions.DependencyInjection` namespace to use the `GetService` method.

# How to Use

MicroEndpoints provides a base class `EndpointBaseAsync` that you can inherit from to define your own endpoints. Here's how you can use it:

```csharp
public class Get : EndpointBaseAsync
      .WithRequest<int>
      .WithIResult
{
  //...
}
```

In this example, the `Get` class is an endpoint that expects a request with an integer parameter and returns an `IResult` type. The actual handling of the request is done in the `HandleAsync` method which needs to be overridden.

# Examples
Here are some examples:

### Get Endpoint

```csharp
public class Get : EndpointBaseAsync
      .WithRequest<int>
      .WithIResult
{
  private IAsyncRepository<Author> _repository;
  private IMapper _mapper;

  public Get(IAsyncRepository<Author> repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  [Get("api/authors/{id}")]
  public override async Task<IResult> HandleAsync([FromServices] IServiceProvider serviceProvider, int id, CancellationToken cancellationToken = default)
  {
    // Implementation...
  }
}
```

### Post Endpoint

```csharp
public class Create : EndpointBaseAsync
    .WithRequest<CreateAuthorCommand>
    .WithIResult
{
  private IAsyncRepository<Author> _repository;
  private IMapper _mapper;

  public Create(IAsyncRepository<Author> repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  [Post("api/authors")]
  public override async Task<IResult> HandleAsync([FromServices] IServiceProvider serviceProvider, [FromBody] CreateAuthorCommand request, CancellationToken cancellationToken = default)
  {
    // Implementation...
  }
}
```

### Put Endpoint

```csharp
public class Create : EndpointBaseAsync
    .WithRequest<CreateAuthorCommand>
    .WithIResult
{
  private IAsyncRepository<Author> _repository;
  private IMapper _mapper;

  public Create(IAsyncRepository<Author> repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  [Put("api/authors")]
  public override async Task<IResult> HandleAsync([FromServices] IServiceProvider serviceProvider, [FromBody] CreateAuthorCommand request, CancellationToken cancellationToken = default)
  {
    // Implementation...
  }
}
```

### Delete Endpoint

```csharp
public class Delete : EndpointBaseAsync
    .WithRequest<int>
    .WithIResult
{
  private IAsyncRepository<Author> _repository;

  public Delete(IAsyncRepository<Author> repository)
  {
    _repository = repository;
  }

  [Delete("api/authors/{id:int}")]
  public override async Task<IResult> HandleAsync([FromServices] IServiceProvider serviceProvider, [FromRoute] int id, CancellationToken cancellationToken = default)
  {
    // Implementation...
  }
}
```

You can see more examples in the [examples](https://github.com/ShadyNagy/MicroEndpoints/tree/main/tests/MicroEndpoints.EndpointApp) directory.

# Acknowledgements

We would like to express our gratitude towards [Ardalis (Steve Smith)](https://github.com/ardalis) for creating the [Ardalis.Endpoints](https://github.com/ardalis/ApiEndpoints) package. His work served as an inspiration and provided a solid foundation for the development of MicroEndpoints. 

We highly recommend checking out his [package](https://github.com/ardalis/ApiEndpoints) and other [contributions](https://github.com/ardalis) to the .NET community.


## Contributing

We welcome contributions! Please see [CONTRIBUTING.md](./CONTRIBUTING.md) for details.

## License

This project is licensed under the terms of the MIT license. See the [LICENSE](./LICENSE) file.
