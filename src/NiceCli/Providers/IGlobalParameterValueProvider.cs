namespace NiceCli.Core;

public interface IGlobalParameterValueProvider
{
  string? GetParameterValue(string name);
}