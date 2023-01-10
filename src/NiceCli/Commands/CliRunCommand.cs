namespace NiceCli.Commands;

/// <summary>
/// This is a marker command used to start the host.
/// </summary>
public sealed class CliRunCommand : ICliCommand
{
  public Task ExecuteAsync()
  {
    throw new InvalidOperationException(
      $"{nameof(CliRunCommand)} is an internal command used to select that the host should run. " +
      "It should not be executed by itself.");
  }
}
