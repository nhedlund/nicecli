namespace NiceCli.Tests.TestDomain;

public class MyGlobalOptions : IMyNumberOptions, IMyLoggingOptions
{
  public int Number { get; set; }
  public bool VerboseLogging { get; set; }
}
