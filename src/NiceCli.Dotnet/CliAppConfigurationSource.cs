using Microsoft.Extensions.Configuration;

namespace NiceCli.Dotnet;

public class CliAppConfigurationSource : IConfigurationSource
{
  private readonly CliApp _cliApp;

  public CliAppConfigurationSource(CliApp cliApp)
  {
    _cliApp = cliApp;
  }

  public IConfigurationProvider Build(IConfigurationBuilder builder)
  {
    return new CliAppConfigurationProvider(_cliApp);
  }
}
