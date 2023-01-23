using NiceCli.Core;

namespace NiceCli;

public class CliCommandDefinition : CliParameter
{
  private const string CommonCommandTypePrefix = "Cli";
  private const string CommonCommandTypeSuffix = "Command";

  protected CliCommandDefinition(
    string description,
    Type commandType,
    CliDefault defaultCommand,
    CliVisibility visibility) :
    base(GetCommandName(commandType), description, CliOptionality.Optional, visibility, new []{GetCommandName(commandType).PascalToKebabCase()})
  {
    CommandType = commandType;
    DefaultCommand = defaultCommand;
  }

  internal Type CommandType { get; }
  internal CliDefault DefaultCommand { get; }
  internal List<CliParameter> Parameters { get; } = new();
  internal List<string> Examples { get; } = new();
  internal List<string> LearnMores { get; } = new();
  internal List<string> CommandArgs { get; } = new();

  internal string CommandName => Name;
  internal string CommandMatchingName => MatchingNames.Single();
  internal IEnumerable<CliParameter> RequiredParameters => Parameters.Where(parameter => parameter.Optionality == CliOptionality.Mandatory);
  internal IEnumerable<CliOption> Options => Parameters.OfType<CliOption>();
  internal IEnumerable<CliFlag> Flags => Parameters.OfType<CliFlag>();
  internal IEnumerable<CliPositionalParameter> PositionalParameters => Parameters.OfType<CliPositionalParameter>();
  internal IEnumerable<CliPositionalParameter> PositionalRequiredParameters => Parameters.OfType<CliPositionalParameter>().Where(parameter => parameter.Optionality == CliOptionality.Mandatory);
  internal IEnumerable<CliPositionalParameter> PositionalNonRequiredParameters => Parameters.OfType<CliPositionalParameter>().Where(parameter => parameter.Optionality == CliOptionality.Optional);
  internal IEnumerable<CliParameter> NonPositionalParameters => Parameters.Where(parameter => parameter is not CliPositionalParameter);

  protected override Action<object, string>? ParseParameter => null;
  protected override Action<object, string>? ParseParameterValue => null;

  internal static string GetCommandName(Type commandType)
  {
    var name = commandType.Name;

    if (name.StartsWith(CommonCommandTypePrefix, StringComparison.OrdinalIgnoreCase))
      name = name.Substring(CommonCommandTypePrefix.Length);

    if (name.EndsWith(CommonCommandTypeSuffix, StringComparison.OrdinalIgnoreCase))
      name = name.Substring(0, name.Length - CommonCommandTypeSuffix.Length);

    return name;
  }
}

public class CliCommandDefinition<TCommand> : CliCommandDefinition where TCommand : ICliCommand
{
  public CliCommandDefinition(
    string description,
    Type commandType,
    CliDefault defaultCommand,
    CliVisibility visibility) :
    base(description, commandType, defaultCommand, visibility)
  {
  }
}
