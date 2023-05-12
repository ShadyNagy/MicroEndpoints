# MicroEndpoints

MicroEndpoints is a .NET library that simplifies creating HTTP endpoints in a minimalistic and clean manner. It's designed to work with .NET 6's Minimal APIs but can also be used in a traditional MVC setup.

## Getting Started

To install the library, you can use the .NET CLI:

~~~shell
dotnet add package MicroEndpoints
~~~

After installing the package, make sure to include the following line in your `Program.cs`:

~~~csharp
//...
builder.Services.AddMicroEndpoints(Assembly.GetAssembly(typeof(Program)));
//...
var app = builder.Build();
//...
app.UseMicroEndpoints();

app.Run();
~~~

# Endpoint Types

MicroEndpoints provides the flexibility to create synchronous or asynchronous endpoints based on your application's needs. 

## Synchronous Endpoints

If you don't have any I/O-bound work that would benefit from asynchrony, you can create synchronous endpoints. Here's an example:

~~~csharp
public class MySyncEndpoint : EndpointBaseSync
    .WithRequest<MyRequest>
    .WithIResult
{
  public override IResult Handle(MyRequest request)
  {
    // Implementation...
  }
}
~~~

In the example above, `MySyncEndpoint` is a synchronous endpoint that handles a `MyRequest` request and returns an `IResult`.

## Asynchronous Endpoints

If you're performing I/O-bound operations, such as network requests or database queries, you should use asynchronous endpoints. Here's an example:

~~~csharp
public class MyAsyncEndpoint : EndpointBaseAsync
    .WithRequest<MyRequest>
    .WithIResult
{
  public override async Task<IResult> HandleAsync(MyRequest request, CancellationToken cancellationToken = default)
  {
    // Implementation...
  }
}
~~~

In this example, `MyAsyncEndpoint` is an asynchronous endpoint that handles a `MyRequest` request and returns an `IResult`.

# Endpoint Results

The result of an endpoint can be of any type. However, it's recommended to use `IResult` for its flexibility and expressiveness. `IResult` allows you to easily produce different HTTP responses.

If you want to return a custom type, you can do so by specifying the type when inheriting from `EndpointBaseAsync` or `EndpointBaseSync`. Here's an example:

~~~csharp
public class MyEndpoint : EndpointBaseAsync
    .WithRequest<MyRequest>
    .WithResult<MyResponse> // MyResponse is a custom class
{
  public override async Task<MyResponse> HandleAsync(MyRequest request, CancellationToken cancellationToken = default)
  {
    // Implementation...
  }
}
~~~

In the example above, `MyEndpoint` is an asynchronous endpoint that handles a `MyRequest` request and returns a `MyResponse` result.

# Dependency Injection

If you need to use services from the dependency injection (DI) container in your endpoints, you can do so within the `Handle` or `HandleAsync` methods. 

You can use the `GetService` method from the `IServiceProvider` to retrieve your services. Here's an example:

~~~csharp
public override async Task<IResult> HandleAsync([FromServices] IServiceProvider serviceProvider, MyRequest request, CancellationToken cancellationToken = default)
{
  // Get services from the DI container
  _repository = serviceProvider.GetService<IAsyncRepository<Author>>()!;
  _mapper = serviceProvider.GetService<IMapper>()!;

  // Implementation...
}
~~~

In the example above, `IAsyncRepository<Author>` and `IMapper` are being retrieved from the service provider, which is injected into the method by the framework.

Remember to add a reference to the `Microsoft.Extensions.DependencyInjection` namespace to use the `GetService` method.

# How to Use

MicroEndpoints provides a base class `EndpointBaseAsync` that you can inherit from to define your own endpoints. Here's how you can use it:

~~~csharp
public class Get : EndpointBaseAsync
      .WithRequest<int>
      .WithIResult
{
  //...
}
~~~

In this example, the `Get` class is an endpoint that expects a request with an integer parameter and returns an `IResult` type. The actual handling of the request is done in the `HandleAsync` method which needs to be overridden.

# Examples
Here are some examples:

### Get Endpoint

~~~csharp
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
~~~

### Post Endpoint

~~~csharp
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
~~~

### Put Endpoint

~~~csharp
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
~~~

### Delete Endpoint

~~~csharp
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
~~~

You can see more examples in the [examples](./examples) directory.

## Contributing

We welcome contributions! Please see [CONTRIBUTING.md](./CONTRIBUTING.md) for details.

## License

This project is licensed under the terms of the MIT license. See the [LICENSE](./LICENSE) file.
