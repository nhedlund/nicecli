using System.Collections;
using NiceCli.Commands;

namespace NiceCli;

public class CliCommands : IReadOnlyList<CliCommandDefinition>
{
  private readonly List<CliCommandDefinition> _commands = new();

  public void Add(CliCommandDefinition command)
  {
    _commands.Add(command);
  }

  public int Count => _commands.Count;
  public bool HasDefaultCommand => _commands.Any(command => command.DefaultCommand == CliDefault.Yes);
  public CliCommandDefinition this[int index] => _commands[index];

  public void AddDefaultCommandsIfNotDefined()
  {
    if (TryGetCommandDefinitionImplementing<ICliHelpCommand>() == null)
      _commands.Add(new CliCommandDefinition<CliHelpCommand>("Show help", typeof(CliHelpCommand), CliDefault.No, CliVisibility.Visible));

    if (TryGetCommandDefinitionImplementing<ICliVersionCommand>() == null)
      _commands.Add(new CliCommandDefinition<CliVersionCommand>("Show version", typeof(CliVersionCommand), CliDefault.No, CliVisibility.Visible));
  }

  internal CliCommandDefinition? TryGetCommandDefinitionImplementing<TCommand>()
  {
    return _commands.SingleOrDefault(command => typeof(TCommand).IsAssignableFrom(command.CommandType));
  }

  public IEnumerator<CliCommandDefinition> GetEnumerator()
  {
    return _commands.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}
