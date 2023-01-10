namespace NiceCli.Tests.TestDomain;

public class MyDefaultRunCommand : ICliCommand, IMyCommand
{
  public bool Verbose { get; set; }
  public int Number { get; set; }
  public int NumberAtExecute { get; set; }

  public Task ExecuteAsync()
  {
    NumberAtExecute = Number;
    return Task.CompletedTask;
  }
}
