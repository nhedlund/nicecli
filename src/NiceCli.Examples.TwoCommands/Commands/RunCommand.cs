namespace NiceCli.Examples.TwoCommands.Commands;

public class RunCommand : ICliCommand
{
  public Task ExecuteAsync()
  {
    Console.WriteLine("Running and exiting");
    return Task.CompletedTask;
  }
}
