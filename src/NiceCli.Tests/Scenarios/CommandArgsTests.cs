using NiceCli.Core;
using NiceCli.Tests.TestDomain;

namespace NiceCli.Tests;

public class CommandArgsTests
{
  private readonly TestContainer _container = new();

  [Test]
  [TestCase("my-default-run", "-n")]
  [TestCase("my-default-run", "--number")]
  [TestCase("my-default-run", "--Number")]
  [TestCase("my-other", "-n")]
  [TestCase("My-other", "--number")]
  [TestCase("my-Other", "--Number")]
  public async Task ArgsContainCommandOptionNumberWithValue_Run_NumberIsSet(string command, string arg)
  {
    var app = command != "" ? CreateCliApp(command, arg, "22") : CreateCliApp(arg, "22");
    app.Parse();

    var exitStatus = await app.RunAsync();

    exitStatus.ShouldBe(0);
    _container.CreatedCommandInstance.ShouldNotBeNull();
    var commandInstance = (IMyCommand) _container.CreatedCommandInstance;
    commandInstance.ShouldNotBeNull();
    commandInstance.Number.ShouldBe(22);
  }

  [Test]
  [TestCase("my-default-run", "-b")]
  [TestCase("my-default-run", "--verbose")]
  [TestCase("my-default-run", "--Verbose")]
  [TestCase("my-other", "-b")]
  [TestCase("my-other", "--verbose")]
  [TestCase("my-other", "--Verbose")]
  public async Task ArgsContainCommandFlag_Run_FlagIsSet(string command, string arg)
  {
    var app = command != "" ? CreateCliApp(command, arg) : CreateCliApp(arg);
    app.Parse();

    var exitStatus = await app.RunAsync();

    exitStatus.ShouldBe(0);
    _container.CreatedCommandInstance.ShouldNotBeNull();
    var commandInstance = (IMyCommand) _container.CreatedCommandInstance;
    commandInstance.ShouldNotBeNull();
    commandInstance.Verbose.ShouldBeTrue();
  }

  private CliApp CreateCliApp(params string[] args)
  {
    var app = CliApp.WithArgs(args)
      .DefaultCommand<MyDefaultRunCommand>("Runs the service",
        o => o.Option(c => c.Number, "Number with a value", "value", 'n')
              .Flag(c => c.Verbose, "Verbose logging", 'b'))
      .Command<MyOtherCommand>("Runs some other stuff",
        o => o.Option(c => c.Number, "Number with a value", "value", 'n')
              .Flag(c => c.Verbose, "Verbose logging", 'b'));

    app.Container = _container;
    return app;
  }

  private class TestContainer : CliInternalContainer
  {
    public override ICliCommand ResolveCommand(Type type)
    {
      CreatedCommandInstance = base.ResolveCommand(type);
      return CreatedCommandInstance;
    }

    public ICliCommand? CreatedCommandInstance { get; private set; }
  }
}
