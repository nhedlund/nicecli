namespace NiceCli.Core;

internal class CliFlag : CliParameter
{
  public CliFlag(string name, char shortName, string longName, string description, CliVisibility visibility, Action<object> parseFlag) :
    base(name, description, CliOptionality.Optional, visibility, new[] {shortName != ' ' ? $"{ShortNamePrefix}{shortName}" : null, $"{LongNamePrefix}{longName}"})
  {
    if (string.IsNullOrWhiteSpace(longName))
      throw new ArgumentException($"{nameof(longName)} is null or empty.");
    if (parseFlag == null)
      throw new ArgumentNullException(nameof(parseFlag));

    ParseParameter = (o, _) => parseFlag(o);
  }

  protected override Action<object, string>? ParseParameter { get; }
  protected override Action<object, string>? ParseParameterValue => null;
}
