using Microsoft.Extensions.Configuration;

namespace NiceCli.Dotnet;

public static class ConfigurationBuilderExtensions
{
  public static IConfigurationBuilder AddCli(this IConfigurationBuilder configurationBuilder, CliApp app)
  {
    configurationBuilder.Add(new CliAppConfigurationSource(app));
    return configurationBuilder;
  }
}
