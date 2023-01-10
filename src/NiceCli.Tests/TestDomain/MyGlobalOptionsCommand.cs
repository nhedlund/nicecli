namespace NiceCli.Tests.TestDomain;

public class MyGlobalOptionsCommand : ICliCommand
{
  public MyGlobalOptionsCommand(MyGlobalOptions myGlobalOptions)
  {
    NumberAtExecute = myGlobalOptions.Number;
  }

  public static int NumberAtExecute { get; set; }

  public Task ExecuteAsync()
  {
    return Task.CompletedTask;
  }
}
