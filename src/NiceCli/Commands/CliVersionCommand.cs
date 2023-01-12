using NiceCli.Core;

namespace NiceCli.Commands;

internal class CliVersionCommand : ICliVersionCommand, ICliBuiltInCommand
{
  private readonly CliDefinition _definition;

  public CliVersionCommand(CliDefinition definition)
  {
    _definition = definition;
  }

  public Task ExecuteAsync()
  {
    if (Console.IsOutputRedirected)
      Console.Write(_definition.AppVersion);
    else
      Console.WriteLine(_definition.AppVersion);

    return Task.CompletedTask;
  }
}
