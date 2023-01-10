namespace NiceCli.Core;

internal class CliPositionalParameter : CliParameter
{
  public CliPositionalParameter(
    string name,
    string description,
    CliOptionality optionality,
    Action<object, string> parseParameter) :
    base(name, description, optionality, CliVisibility.Visible, Enumerable.Empty<string?>())
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException($"{nameof(name)} is null or empty.");

    ParseParameter = parseParameter ?? throw new ArgumentNullException(nameof(parseParameter));
  }

  protected override Action<object, string>? ParseParameter { get; }
  protected override Action<object, string>? ParseParameterValue => null;
}
