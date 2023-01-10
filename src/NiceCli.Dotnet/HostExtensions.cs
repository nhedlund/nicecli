using Microsoft.Extensions.Hosting;

namespace NiceCli.Dotnet;

public static class HostExtensions
{
  public static async Task<int> RunCliCommandAsync<T>(this T host, CliApp cliApp) where T : IHost
  {
    cliApp.AddServiceProvider(host.Services);
    cliApp.RegisterHostRun(() => RunAsync(host));
    var status = await cliApp.RunAsync();
    return status;
  }

  private static async Task<int> RunAsync<T>(T host) where T : IHost
  {
    await host.RunAsync();
    return 0;
  }
}
