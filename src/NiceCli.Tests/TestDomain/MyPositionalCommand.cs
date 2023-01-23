namespace NiceCli.Tests.TestDomain;

public class MyPositionalCommand : ICliCommand
{
  public DateTime Start { get; set; }

  public DateTime End { get; set; }

  public Task ExecuteAsync()
  {
    return Task.CompletedTask;
  }
}
