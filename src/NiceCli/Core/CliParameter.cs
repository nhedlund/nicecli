namespace NiceCli.Core;

public abstract class CliParameter
{
  internal const string ShortNamePrefix = "-";
  internal const string LongNamePrefix = "--";

  protected CliParameter(
    string name,
    string description,
    CliOptionality optionality,
    CliVisibility visibility,
    IEnumerable<string?> names)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException($"{nameof(name)} is null or empty.");
    if (string.IsNullOrWhiteSpace(description))
      throw new ArgumentException($"{nameof(description)} is null or empty.");
    if (names == null)
      throw new ArgumentNullException(nameof(names));

    MatchingNames = names
      .Where(n => !string.IsNullOrWhiteSpace(n))
      .Select(n => (n ?? "").Trim())
      .ToList();

    Name = name;
    Optionality = optionality;
    Visibility = visibility;
    Description = description;
  }

  protected abstract Action<object, string>? ParseParameter { get; }
  protected abstract Action<object, string>? ParseParameterValue { get; }

  internal int ParameterArgumentCount => ParseParameterValue != null ? 2 : 1;

  /// <summary>
  /// Parameter name in Pascal case.
  /// </summary>
  internal string Name { get; }

  /// <summary>
  /// Parameter names that match this parameter to the CLI arguments, for example "start", "-v" or "--version".
  /// If empty this is a positional parameter, probably a required command parameter.
  /// </summary>
  internal IReadOnlyList<string> MatchingNames { get; }

  /// <summary>
  /// Description of command, option or flag.
  /// </summary>
  internal string Description { get; }

  /// <summary>
  /// If this parameter is mandatory or not.
  /// </summary>
  internal CliOptionality Optionality { get; }

  /// <summary>
  /// If this parameter is hidden or shown in help text.
  /// </summary>
  /// <example>
  /// For example if this is a command and it is hidden, then it is not shown in the main help text that shows
  /// commands and global options, but help text is shown if you ask for help for this specific command.
  /// </example>
  internal CliVisibility Visibility { get; }

  /// <summary>
  /// Parameter raw value (the argument), null if parameter was not used.
  /// For flags this is either null (no flag), or <see cref="bool.TrueString"/>.
  /// </summary>
  internal string? Value { get; set; }

  /// <summary>
  /// Map value (if set) to bound object.
  /// The optional binding has been setup earlier by the child class.
  /// </summary>
  /// <param name="obj"></param>
  internal void MapValueToObjectIfSet(object obj)
  {
    if (Value != null)
    {
      ParseParameter?.Invoke(obj, Value);
      ParseParameterValue?.Invoke(obj, Value);
    }
  }

  public override string ToString()
  {
    return $"{Name}{(Value != null ? $": {Value}" : "")}";
  }
}
