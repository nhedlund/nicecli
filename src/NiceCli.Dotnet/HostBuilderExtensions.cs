using Microsoft.Extensions.Hosting;

namespace NiceCli.Dotnet;

public static class HostBuilderExtensions
{
  public static IHostBuilder ConfigureCli(this IHostBuilder hostBuilder, CliApp cliApp)
  {
    hostBuilder.ConfigureAppConfiguration((_, configuration) => configuration.AddCli(cliApp));
    return hostBuilder;
  }
}
