using Microsoft.Extensions.Hosting;
using NiceCli;
using NiceCli.Dotnet;

// CLI app has one default command that runs the host

var cliApp = CliApp.WithArgs(args)
  .CommandRun("Starts stuff", CliDefault.Yes);

using var host = Host.CreateDefaultBuilder()
  .ConfigureCli(cliApp)
  .Build();

return await host.RunCliCommandAsync(cliApp);
