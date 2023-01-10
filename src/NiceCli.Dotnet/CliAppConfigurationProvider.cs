using System.Collections;
using Microsoft.Extensions.Configuration;

namespace NiceCli.Dotnet;

public class CliAppConfigurationProvider : ConfigurationProvider, IEnumerable<KeyValuePair<string, string>>
{
  public CliAppConfigurationProvider(CliApp cliApp)
  {
    if (cliApp == null)
      throw new ArgumentNullException(nameof(cliApp));

    foreach (var globalParameter in cliApp.GetGlobalParameterValues())
    {
      Add(globalParameter.Key, globalParameter.Value);
    }
  }

  public void Add(string key, string value)
  {
    Data.Add(key, value);
  }

  public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
  {
    return Data.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}
