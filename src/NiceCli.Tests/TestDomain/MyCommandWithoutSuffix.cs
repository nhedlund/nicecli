namespace NiceCli.Tests.TestDomain;

public class MyCommandWithoutSuffix : ICliCommand
{
  public Task ExecuteAsync()
  {
    return Task.CompletedTask;
  }
}
