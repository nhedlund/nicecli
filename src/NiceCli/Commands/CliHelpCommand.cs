using NiceCli.Core;

namespace NiceCli.Commands;

internal class CliHelpCommand : ICliHelpCommand, ICliBuiltInCommand
{
  internal const string ParameterNameSeparator = ", ";
  private const int MaxParameterWidthLimit = 40;
  private const string Indent = "  ";
  private const int MinimumColumnMargin = 2;
  private readonly CliDefinition _definition;
  private readonly CliCommandDefinition? _selectedCommand;
  private readonly CliCommandDefinition? _defaultCommand;
  private readonly int _maxParameterWidth;
  private readonly bool _useAnsiCodes;
  private readonly bool _hasHiddenCommands;

  public CliHelpCommand(CliDefinition definition, CliSelectedCommand selectedCommand)
  {
    _definition = definition ?? throw new ArgumentNullException(nameof(definition));
    _selectedCommand = selectedCommand.SelectedCommandToShowHelpFor;
    _useAnsiCodes = !Console.IsOutputRedirected;
    _defaultCommand = _definition.Commands.SingleOrDefault(command => command.DefaultCommand == CliDefault.Yes);
    _hasHiddenCommands = _definition.Commands.Any(command => command.Visibility == CliVisibility.Hidden);
    _maxParameterWidth = GetMaxParameterWidth(definition);
  }

  public Task ExecuteAsync()
  {
    if (_selectedCommand == null)
      ShowGeneralUsage();
    else
      ShowCommandUsage(_selectedCommand);

    return Task.CompletedTask;
  }

  private int GetMaxParameterWidth(CliDefinition definition)
  {
    var maxWidth = _selectedCommand != null ?
      _selectedCommand.Parameters.GetMaxWidth() :
      definition.Options.Parameters.GetMaxWidth();

    return Math.Min(maxWidth, MaxParameterWidthLimit);
  }

  private void ShowGeneralUsage()
  {
    WriteDescription(_definition.Description);
    WriteAppUsage();
    WriteDefaultCommand();
    WriteNonDefaultCommands();
    WriteOptions();
    WriteFlags();
    WriteExamples();
    WriteLearnMore();
  }

  private void ShowCommandUsage(CliCommandDefinition commandDefinition)
  {
    WriteDescription(commandDefinition.Description);
    WriteCommandUsage(commandDefinition);
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
    WriteSection("Usage", $"{_definition.Name} {_definition.Usage}");
  }

  private void WriteCommandUsage(CliCommandDefinition commandDefinition)
  {
    WriteSection("Usage", $"{_definition.Name} {commandDefinition.CommandMatchingName}");
  }

  private void WriteDefaultCommand()
  {
    if (_defaultCommand != null)
      WriteSection("Default Command", GetParameterDescription(_defaultCommand));
  }

  private void WriteNonDefaultCommands()
  {
    var commands = _definition.Commands.Where(command =>
      command != _defaultCommand &&
      command.DefaultCommand == CliDefault.No &&
      command.Visibility == CliVisibility.Visible).ToList();

    WriteSection(_hasHiddenCommands ? "Common Commands" : "Commands", commands.Select(GetParameterDescription).ToArray());
  }

  private void WriteOptions()
  {
    var options = _definition.Options.Options.Where(option => option.Visibility == CliVisibility.Visible).ToList();
    WriteSection("Options", options.Select(GetParameterDescription).ToArray());
  }

  private void WriteFlags()
  {
    var flags = _definition.Options.Flags.Where(option => option.Visibility == CliVisibility.Visible).ToList();
    WriteSection("Flags", flags.Select(GetParameterDescription).ToArray());
  }

  private void WriteExamples()
  {
    WriteSection("Examples", _definition.Examples.ToArray());
  }

  private void WriteLearnMore()
  {
    WriteSection("Learn More", _definition.LearnMore.ToArray());
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
