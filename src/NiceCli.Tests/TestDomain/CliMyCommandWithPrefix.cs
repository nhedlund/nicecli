namespace NiceCli.Tests.TestDomain;

public class CliMyCommandWithPrefix : ICliCommand
{
  public Task ExecuteAsync()
  {
    return Task.CompletedTask;
  }
}
