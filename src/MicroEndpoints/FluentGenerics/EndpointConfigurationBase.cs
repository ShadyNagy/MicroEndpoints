using System.Linq.Expressions;
using System.Reflection;
using MicroEndpoints.Interfaces;
using Microsoft.AspNetCore.Builder;

namespace MicroEndpoints.FluentGenerics;

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