using NiceCli.Commands;

namespace NiceCli;

public class CliSelectedCommand
{
  private readonly CliGlobalOptions _globalOptions;
  private readonly CliCommands _commands;
  private CliCommandDefinition? _selectedCommand;

  public CliSelectedCommand(CliGlobalOptions globalOptions, CliCommands commands, CliCommandDefinition? selectedFromArgs)
  {
    _globalOptions = globalOptions;
    _commands = commands;
    SelectCommand(selectedFromArgs);
  }

  public Type CommandType => _selectedCommand!.CommandType;
  internal CliCommandDefinition? SelectedCommandToShowHelpFor { get; private set; }

  private void SelectCommand(CliCommandDefinition? selectedFromArgs)
  {
    _selectedCommand = selectedFromArgs;

    SetVersionCommandIfVersionFlagIsPresent();
    SetHelpForCommandOrGeneralHelpIfHelpFlagIsPresent();
    SetDefaultCommandAsSelectedIfAvailableAndNoCommandIsSet();
    SetHelpIfNoSelectedCommand();
  }

  private void SetVersionCommandIfVersionFlagIsPresent()
  {
    if (_globalOptions.IsVersionRequested)
      _selectedCommand = _commands.TryGetCommandDefinitionImplementing<ICliVersionCommand>();
  }

  private void SetHelpForCommandOrGeneralHelpIfHelpFlagIsPresent()
  {
    if (_globalOptions.IsHelpRequested)
    {
      SelectedCommandToShowHelpFor = _selectedCommand;
      _selectedCommand = _commands.TryGetCommandDefinitionImplementing<ICliHelpCommand>();
    }
  }

  private void SetHelpIfNoSelectedCommand()
  {
    if (_selectedCommand == null)
      _selectedCommand = _commands.TryGetCommandDefinitionImplementing<ICliHelpCommand>();
  }

  private void SetDefaultCommandAsSelectedIfAvailableAndNoCommandIsSet()
  {
    if (_selectedCommand == null)
      _selectedCommand = _commands.SingleOrDefault(command => command.DefaultCommand == CliDefault.Yes);
  }

  internal void BindCommand(object commandInstance)
  {
    _selectedCommand?.Parameters.ForEach(parameter => parameter.MapValueToObjectIfSet(commandInstance));
  }

}
