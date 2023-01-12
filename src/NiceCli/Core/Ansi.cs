namespace NiceCli.Core;

internal static class Ansi
{
  /// <summary>
  /// Wrap <param name="text"></param> in bold ANSI terminal codes.
  /// </summary>
  public static string Bold(string text)
  {
    return $"\u001b[1m{text}\u001b[0m";
  }
}
