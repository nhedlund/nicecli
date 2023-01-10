namespace NiceCli.Core;

internal class EnvironmentVariableProvider : IGlobalParameterValueProvider
{
  private readonly Dictionary<string,string> _environmentVariables;

  public EnvironmentVariableProvider(IEnvironmentVariableSource environmentVariableSource, string prefix)
  {
    var environmentVariables = environmentVariableSource.GetEnvironmentVariables();

    _environmentVariables = environmentVariables
      .Where(kv => kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
      .ToDictionary(kv => kv.Key.Substring(prefix.Length).Replace("_", ""), kv => kv.Value, StringComparer.OrdinalIgnoreCase);
  }

  public string? GetParameterValue(string name)
  {
    return _environmentVariables.TryGetValue(name, out var value) ? value : null;
  }
}
