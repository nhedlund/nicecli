namespace NiceCli.Core;

internal class EnvironmentVariableSource : IEnvironmentVariableSource
{
  public Dictionary<string, string> GetEnvironmentVariables()
  {
    var keys = Environment.GetEnvironmentVariables().Keys;

    var keyValues = keys
      .Cast<string>()
      .Select(key => new KeyValuePair<string, string>(key, Environment.GetEnvironmentVariable(key)));

    return keyValues
      .Where(keyValue => keyValue.Value != null)
      .ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value);
  }
}
