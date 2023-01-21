using NiceCli.Core;

namespace NiceCli.Commands;

internal class CliVersionCommand : ICliVersionCommand, ICliBuiltInCommand
{
  private readonly CliAppDefinition _appDefinition;

  public CliVersionCommand(CliAppDefinition appDefinition)
  {
    _appDefinition = appDefinition;
  }

  public Task ExecuteAsync()
  {
    if (Console.IsOutputRedirected)
      Console.Write(_appDefinition.AppVersion);
    else
      Console.WriteLine(_appDefinition.AppVersion);

    return Task.CompletedTask;
  }
}
