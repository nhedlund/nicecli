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
    Console.Write(_definition.AppVersion);
    return Task.CompletedTask;
  }
}
