using NiceCli.Commands;

namespace NiceCli.Core;

internal static class CliParameterValidator
{
  public static void ValidateDefinition(IReadOnlyList<CliParameter> globalParameters, IReadOnlyList<CliCommandDefinition> commands)
  {
    ValidateAtLeastOneCommandExists(commands);
    ValidateThatAtMostOneDefaultCommandExists(commands);
    ValidateRootParameterUniqueness(globalParameters, commands);
    ValidateRootVsCommandParameterUniqueness(globalParameters, commands);
    ValidateDefaultCommandParameters(commands);
    ValidateThatExactlyOneCommandExistsOfType<ICliHelpCommand>(commands);
    ValidateThatExactlyOneCommandExistsOfType<ICliVersionCommand>(commands);
  }

  private static void ValidateAtLeastOneCommandExists(IEnumerable<CliCommandDefinition> commands)
  {
    if (!commands.Any())
      throw new InvalidOperationException("At least one command must be added to the CLI, so it has something to do.");
  }

  private static void ValidateThatAtMostOneDefaultCommandExists(IEnumerable<CliCommandDefinition> commands)
  {
    if (commands.Count(command => command.DefaultCommand == CliDefault.Yes) > 1)
      throw new InvalidOperationException("More than one command is default. Only 0 or 1 command can be default in the CLI.");
  }

  private static void ValidateThatExactlyOneCommandExistsOfType<TCommand>(IEnumerable<CliCommandDefinition> commands)
  {
    var commandCount = commands.Count(command => typeof(TCommand).IsAssignableFrom(command.CommandType));

    if (commandCount != 1)
      throw new InvalidOperationException($"Found {commandCount} implementing type {typeof(TCommand).FullName} where exactly one should be registered.");
  }

  private static void ValidateRootParameterUniqueness(IEnumerable<CliParameter> globalParameters, IEnumerable<CliCommandDefinition> commands)
  {
    var allRootParameters = globalParameters.Union(commands);
    var allRootNames = allRootParameters.SelectMany(parameter => parameter.MatchingNames).ToList();
    var nonUniqueNames = allRootNames.GroupBy(name => name).Where(g => g.Count() > 1).ToList();

    if (nonUniqueNames.Any())
      throw new InvalidOperationException($"Found parameter names that are non-unique: {string.Join(", ", nonUniqueNames.Select(g => g.Key))}");
  }

  private static void ValidateRootVsCommandParameterUniqueness(IEnumerable<CliParameter> globalParameters, IEnumerable<CliCommandDefinition> commands)
  {
    var allRootOptionNames = new HashSet<string>(globalParameters.SelectMany(parameter => parameter.MatchingNames.Select(name => name.ToLowerInvariant())));

    var rootOptionNamesFoundInCommands = commands.Where(command =>
        command.Parameters.Any(parameter =>
          parameter.MatchingNames.Any(name => allRootOptionNames.Contains(name.ToLowerInvariant()))))
      .ToList();

    if (rootOptionNamesFoundInCommands.Any())
      throw new InvalidOperationException($"Found root parameter names that are duplicated in commands: {string.Join(", ", rootOptionNamesFoundInCommands)}");
  }

  private static void ValidateDefaultCommandParameters(IEnumerable<CliCommandDefinition> commands)
  {
    var defaultCommand = commands.SingleOrDefault(command => command.DefaultCommand == CliDefault.Yes);

    if (defaultCommand != null && defaultCommand.Parameters.Any(parameter => parameter.Optionality == CliOptionality.Mandatory))
      throw new InvalidOperationException("Mandatory parameters is not allowed on a default command.");
  }
}
