using System.Text.RegularExpressions;

namespace NiceCli.Core;

internal static class StringExtensions
{
  public static string PascalToKebabCase(this string value)
  {
    if (string.IsNullOrEmpty(value))
      return value;

    return Regex.Replace(value, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])", "-$1")
      .Trim()
      .ToLowerInvariant();
  }
}
