namespace NiceCli.Tests.TestDomain;

public interface IVerboseAndNumberCommand
{
  bool Verbose { get; set; }
  int Number { get; set; }
}
