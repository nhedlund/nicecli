namespace NiceCli.Core;

internal class CliOption : CliParameter
{
  public CliOption(string name, char shortName, string longName, string parameter, string description, CliVisibility visibility, Action<object, string> parseValue) :
    base(name, description, CliOptionality.Optional, visibility, new[] {shortName != ' ' ? $"{ShortNamePrefix}{shortName}" : null, $"{LongNamePrefix}{longName}"})
  {
    if (string.IsNullOrWhiteSpace(longName))
      throw new ArgumentException($"{nameof(longName)} is null or empty.");

    Parameter = parameter;
    ParseParameterValue = parseValue ?? throw new ArgumentNullException(nameof(parseValue));
  }

  public string Parameter { get; }
  protected override Action<object, string>? ParseParameter => null;
  protected override Action<object, string>? ParseParameterValue { get; }

  internal override int DefinitionWidth => base.DefinitionWidth + 1 + Parameter.Length;
}
