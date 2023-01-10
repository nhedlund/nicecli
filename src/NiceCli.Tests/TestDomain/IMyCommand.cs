namespace NiceCli.Tests.TestDomain;

public interface IMyCommand
{
  bool Verbose { get; set; }
  int Number { get; set; }
}
