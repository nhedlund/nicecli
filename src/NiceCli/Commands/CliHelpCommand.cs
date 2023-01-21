using NiceCli.Core;

namespace NiceCli.Commands;

internal class CliHelpCommand : ICliHelpCommand, ICliBuiltInCommand
{
  internal const string ParameterNameSeparator = ", ";
  private const int MaxParameterWidthLimit = 40;
  private const string Indent = "  ";
  private const int MinimumColumnMargin = 2;
  private readonly CliAppDefinition _appDefinition;
  private readonly CliCommandDefinition? _selectedCommand;
  private readonly CliCommandDefinition? _defaultCommand;
  private readonly int _maxParameterWidth;
  private readonly bool _useAnsiCodes;
  private readonly bool _hasHiddenCommands;

  public CliHelpCommand(CliAppDefinition appDefinition, CliSelectedCommand selectedCommand)
  {
    _appDefinition = appDefinition ?? throw new ArgumentNullException(nameof(appDefinition));
    _selectedCommand = selectedCommand.SelectedCommandToShowHelpFor;
    _useAnsiCodes = !Console.IsOutputRedirected;
    _defaultCommand = _appDefinition.Commands.SingleOrDefault(command => command.DefaultCommand == CliDefault.Yes);
    _hasHiddenCommands = _appDefinition.Commands.Any(command => command.Visibility == CliVisibility.Hidden);
    _maxParameterWidth = GetMaxParameterWidth(appDefinition);
  }

  public Task ExecuteAsync()
  {
    if (_selectedCommand == null)
      ShowGeneralUsage();
    else
      ShowCommandUsage(_selectedCommand);

    return Task.CompletedTask;
  }

  private int GetMaxParameterWidth(CliAppDefinition appDefinition)
  {
    var maxWidth = _selectedCommand != null ?
      _selectedCommand.Parameters.GetMaxWidth() :
      appDefinition.Options.Parameters.GetMaxWidth();

    return Math.Min(maxWidth, MaxParameterWidthLimit);
  }

  private void ShowGeneralUsage()
  {
    WriteDescription(_appDefinition.Description);
    WriteAppUsage();
    WriteDefaultCommand();
    WriteNonDefaultCommands();
    WriteOptions(_appDefinition.Options.Options);
    WriteFlags(_appDefinition.Options.Flags);
    WriteExamples(_appDefinition.Examples);
    WriteLearnMore(_appDefinition.LearnMores);
  }

  private void ShowCommandUsage(CliCommandDefinition commandDefinition)
  {
    WriteDescription(commandDefinition.Description);
    WriteCommandUsage(commandDefinition);
    WriteOptions(commandDefinition.Options);
    WriteFlags(commandDefinition.Flags);
    WriteExamples(commandDefinition.Examples);
    WriteLearnMore(commandDefinition.LearnMores);
  }

  private static void WriteDescription(string description)
  {
    if (string.IsNullOrWhiteSpace(description))
      return;

    WriteLine(description);
    WriteLine();
  }

  private void WriteAppUsage()
  {
    WriteSection("Usage", $"{_appDefinition.Name} {_appDefinition.Usage}");
  }

  private void WriteCommandUsage(CliCommandDefinition commandDefinition)
  {
    var optionalArgs = commandDefinition.Parameters.Any() ? " [args]" : "";
    WriteSection("Usage", $"{_appDefinition.Name} {commandDefinition.CommandMatchingName}{optionalArgs}");
  }

  private void WriteDefaultCommand()
  {
    if (_defaultCommand != null)
      WriteSection("Default Command", GetParameterDescription(_defaultCommand));
  }

  private void WriteNonDefaultCommands()
  {
    var commands = _appDefinition.Commands.Where(command =>
      command != _defaultCommand &&
      command.DefaultCommand == CliDefault.No &&
      command.Visibility == CliVisibility.Visible).ToList();

    WriteSection(_hasHiddenCommands ? "Common Commands" : "Commands", commands.Select(GetParameterDescription).ToArray());
  }

  private void WriteOptions(IEnumerable<CliOption> options)
  {
    options = options.Where(option => option.Visibility == CliVisibility.Visible);
    WriteSection("Options", options.Select(GetParameterDescription).ToArray());
  }

  private void WriteFlags(IEnumerable<CliFlag> flags)
  {
    flags = flags.Where(option => option.Visibility == CliVisibility.Visible);
    WriteSection("Flags", flags.Select(GetParameterDescription).ToArray());
  }

  private void WriteExamples(IEnumerable<string> examples)
  {
    WriteSection("Examples", examples.ToArray());
  }

  private void WriteLearnMore(IEnumerable<string> learnMores)
  {
    WriteSection("Learn More", learnMores.ToArray());
  }

  private string GetParameterDescription(CliParameter parameter)
  {
    var names = string.Join(ParameterNameSeparator, parameter.MatchingNames);
    var namesWithOptionalValue = parameter is CliOption option ? $"{names} {option.Parameter}" : names;
    var namesTotalWidth = _maxParameterWidth + MinimumColumnMargin;

    return $"{namesWithOptionalValue.PadRight(namesTotalWidth)}{parameter.Description}";
  }

  private void WriteSection(string header, params string[] rows)
  {
    if (rows.Length == 0)
      return;

    WriteHeader(header);

    foreach (var row in rows)
    {
      WriteIndentedLine(row);
    }

    WriteLine();
  }

  private void WriteHeader(string header)
  {
    header = header.ToUpperInvariant();

    if (_useAnsiCodes)
      header = Ansi.Bold(header);

    Console.WriteLine(header);
  }

  private static void WriteIndentedLine(string row = "")
  {
    WriteLine($"{Indent}{row}");
  }

  private static void WriteLine(string row = "")
  {
    Console.WriteLine(row);
  }
}
