namespace NiceCli.Core;

internal static class CliParameterExtensions
{
  internal static bool HasValue(this CliParameter parameter)
  {
    return parameter.Value != null;
  }

  internal static bool ParameterNameEqualsArgument(this CliParameter parameter, string arg)
  {
    return parameter.MatchingNames.Any(name =>
      name.Equals(arg, IsShortName(arg) ? StringComparison.Ordinal: StringComparison.OrdinalIgnoreCase));
  }

  private static bool IsShortName(string arg)
  {
    return arg.Length == 2 && arg[0] == CliParameter.ShortNamePrefix[0];
  }

  internal static int GetMaxWidth(this IEnumerable<CliParameter> parameters)
  {
    var maxWidth = 0;

    foreach (var parameter in parameters)
    {
      var width = parameter.DefinitionWidth;

      if (width > maxWidth)
        maxWidth = width;
    }

    return maxWidth;
  }
}
