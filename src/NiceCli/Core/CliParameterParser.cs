namespace NiceCli.Core;

internal static class CliParameterParser
{
  /// <summary>
  /// Parse args and set the raw values of the parameters that are used.
  /// </summary>
  /// <returns>
  /// Selected command if specified in the args and any unknown args that could not
  /// be mapped to the defined global parameters or the optional selected command.
  /// </returns>
  public static CliCommandDefinition? ParseParameters(
    IEnumerable<string> args,
    IEnumerable<CliParameter> globalParameters,
    IEnumerable<CliCommandDefinition> commands)
  {
    var unprocessedArgs = args.ToList();

    ParseGlobalParameters(globalParameters, unprocessedArgs);
    var selectedCommand = ParseOptionalSelectedCommand(commands, unprocessedArgs);

    if (selectedCommand != null)
      ParseCommandParameters(selectedCommand);

    EnsureNoUnprocessedArgsRemain(unprocessedArgs);
    return selectedCommand;
  }

  private static void ParseGlobalParameters(IEnumerable<CliParameter> globalParameters, List<string> unprocessedArgs)
  {
    ParseParameters(unprocessedArgs, globalParameters);
  }

  private static void ParseParameters(List<string> unprocessedArgs, IEnumerable<CliParameter> parameters)
  {
    parameters.ForEach(parameter =>
      ParseParameterIfFoundInUnprocessedArgs(unprocessedArgs, parameter));
  }

  private static CliCommandDefinition? ParseOptionalSelectedCommand(IEnumerable<CliCommandDefinition> commands, List<string> unprocessedArgs)
  {
    // Unprocessed args should now contain: [command] [command args]
    if (unprocessedArgs.Count == 0)
      return null;

    var commandName = new List<string> {unprocessedArgs.First()};

    foreach (var command in commands)
    {
      if (ParseParameterIfFoundInUnprocessedArgs(commandName, command))
      {
        unprocessedArgs.RemoveAt(0);
        AssignUnprocessedArgsToCommand(unprocessedArgs, command);
        return command;
      }
    }

    return null;
  }

  private static void ParseCommandParameters(CliCommandDefinition command)
  {
    var argsCount = command.CommandArgs.Count;
    var requiredParameterCount = command.RequiredParameters.Sum(parameter => parameter.ParameterArgumentCount);

    if (argsCount < requiredParameterCount)
      throw new CliUserException($"Command {command.CommandName} requires {requiredParameterCount} arguments, but only {argsCount} arguments were used.");

    var argsToPreValidate = command.CommandArgs.ToList();
    ParseParameters(argsToPreValidate, command.Parameters);

    if (argsToPreValidate.Any())
      throw new CliUserException($"Command {command.CommandName} got unknown parameters: {string.Join(", ", argsToPreValidate)}");
  }

  private static void AssignUnprocessedArgsToCommand(List<string> unprocessedArgs, CliCommandDefinition command)
  {
    command.CommandArgs.AddRange(unprocessedArgs);
    unprocessedArgs.Clear();
  }

  private static bool ParseParameterIfFoundInUnprocessedArgs(List<string> unprocessedArgs, CliParameter parameter)
  {
    for (var i = 0; i < unprocessedArgs.Count; i++)
    {
      var arg = unprocessedArgs[i];

      if (parameter.ParameterNameEqualsArgument(arg))
      {
        if (parameter.ParameterArgumentCount == 2)
        {
          if (i + 1 >= unprocessedArgs.Count)
            throw new InvalidOperationException($"Option {arg} requires a second parameter, see help.");

          var value = unprocessedArgs[i + 1];
          parameter.Value = value;
          unprocessedArgs.RemoveAt(i + 1);
          unprocessedArgs.RemoveAt(i);
        }
        else
        {
          parameter.Value = parameter is CliFlag ? bool.TrueString : arg;
          unprocessedArgs.RemoveAt(i);
        }

        return true;
      }
    }

    return false;
  }

  private static void EnsureNoUnprocessedArgsRemain(List<string> unprocessedArgs)
  {
    if (unprocessedArgs.Any())
      throw new CliUserException($"No known command selected, and got unknown parameters: {string.Join(", ", unprocessedArgs)}");
  }
}
