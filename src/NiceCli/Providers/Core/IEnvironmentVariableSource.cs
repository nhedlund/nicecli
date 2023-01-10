namespace NiceCli.Core;

internal interface IEnvironmentVariableSource
{
  Dictionary<string, string> GetEnvironmentVariables();
}
