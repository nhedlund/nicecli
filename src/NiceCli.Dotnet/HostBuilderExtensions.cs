using Microsoft.Extensions.Hosting;

namespace NiceCli.Dotnet;

public static class HostBuilderExtensions
{
  internal const string GlobalOptionsKey = $"Host{nameof(CliGlobalOptions)}";

  public static IHostBuilder ConfigureCli(this IHostBuilder hostBuilder, CliApp cliApp)
  {
    hostBuilder.Properties[GlobalOptionsKey] = cliApp.GetGlobalOptions();
    hostBuilder.ConfigureAppConfiguration((_, configuration) => configuration.AddCli(cliApp));
    return hostBuilder;
  }
}
