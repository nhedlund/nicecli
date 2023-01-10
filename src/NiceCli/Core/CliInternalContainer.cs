namespace NiceCli.Core;

internal class CliInternalContainer
{
  private readonly Dictionary<Type, object> _singletons = new();
  private readonly HashSet<Type> _commandTypes = new();
  private IServiceProvider? _externalServiceProvider;

  public void AddExternalServiceProvider(IServiceProvider serviceProvider)
  {
    if (_externalServiceProvider != null)
    {
      throw new InvalidOperationException(
        "Cannot add service provider, a service provider has already been added. " +
        $"{nameof(CliInternalContainer)} can only have one external service provider.");
    }

    _externalServiceProvider = serviceProvider;
  }

  public void AddSingleton(Type type, object instance)
  {
    _singletons[type] = instance;
  }

  public void AddCommand(Type type)
  {
    _commandTypes.Add(type);
  }

  public virtual ICliCommand ResolveCommand(Type type)
  {
    if (!_commandTypes.Contains(type))
      throw new KeyNotFoundException($"CLI command has not been registered: {type.FullName}");

    var constructors = type.GetConstructors();

    if (constructors.Length == 0)
      throw new InvalidOperationException($"CLI command has no public constructor, please add one: {type.FullName}");

    if (constructors.Length > 1)
      throw new InvalidOperationException($"CLI command has more than one public constructor, only one is supported: {type.FullName}");

    var constructorParameters = constructors.Single().GetParameters();
    var constructorParameterValues = constructorParameters.Select(parameter => ResolveDependency(parameter.ParameterType)).ToArray();
    var command = (ICliCommand) Activator.CreateInstance(type, constructorParameterValues);

    return command;
  }

  private object ResolveDependency(Type serviceType)
  {
    if (_singletons.TryGetValue(serviceType, out var instance))
      return instance;

    var externalInstance = _externalServiceProvider?.GetService(serviceType);

    if (externalInstance != null)
      return externalInstance;

    throw new KeyNotFoundException($"Could not resolve type, it has not been registered in this container or optional external container: {serviceType.FullName}");
  }
}
