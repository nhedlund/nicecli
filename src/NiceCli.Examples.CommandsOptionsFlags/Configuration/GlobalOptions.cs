namespace NiceCli.Examples.CommandsOptionsFlags;

public class GlobalOptions : IDatabaseOptions, ILoggingOptions
{
  public string Db { get; set; } = "Server=localhost;Database=ExampleDatabase;Trusted_Connection=True;";

  public TimeSpan CommandTimeout { get; set; } = TimeSpan.FromSeconds(60);

  public bool Verbose { get; set; }
}
