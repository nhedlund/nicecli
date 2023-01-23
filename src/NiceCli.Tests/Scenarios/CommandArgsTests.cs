using System.Globalization;
using NiceCli.Commands;
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
    var commandInstance = (IVerboseAndNumberCommand) _container.CreatedCommandInstance;
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
    var commandInstance = (IVerboseAndNumberCommand) _container.CreatedCommandInstance;
    commandInstance.ShouldNotBeNull();
    commandInstance.Verbose.ShouldBeTrue();
  }

  [Test]
  public async Task ArgsContainHelpAndCommand_Run_HelpCommandIsSelected()
  {
    var app = CreateCliApp("my-default-run", "-h");
    app.Parse();

    var exitStatus = await app.RunAsync();

    exitStatus.ShouldBe(0);
    _container.CreatedCommandInstance.ShouldNotBeNull();
    var commandInstance = (CliHelpCommand) _container.CreatedCommandInstance;
    commandInstance.ShouldNotBeNull();
  }

  [Test]
  public async Task PositionalArgsCommand_Run_DatesAreSet()
  {
    var expectedStart = new DateTime(2022, 01, 25);
    var expectedEnd = new DateTime(2024, 11, 29);
    var app = CreateCliApp(
      "my-positional",
      expectedStart.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
      expectedEnd.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    app.Parse();

    var exitStatus = await app.RunAsync();

    exitStatus.ShouldBe(0);
    _container.CreatedCommandInstance.ShouldNotBeNull();
    var commandInstance = (MyPositionalCommand) _container.CreatedCommandInstance;
    commandInstance.ShouldNotBeNull();
    commandInstance.Start.ShouldBe(expectedStart);
    commandInstance.End.ShouldBe(expectedEnd);
  }

  private CliApp CreateCliApp(params string[] args)
  {
    var app = CliApp.WithArgs(args)
      .DefaultCommand<MyDefaultRunCommand>("Runs the service", o => o
        .Option(c => c.Number, "Number with a value", "value", 'n')
        .Flag(c => c.Verbose, "Verbose logging", 'b'))
      .Command<MyOtherCommand>("Runs some other stuff", o => o
        .Option(c => c.Number, "Number with a value", "value", 'n')
        .Flag(c => c.Verbose, "Verbose logging", 'b'))
      .Command<MyPositionalCommand>("Contains two positional (required) parameters", o => o
        .PositionalParameter(c => c.Start, "Start date")
        .PositionalParameter(c => c.End, "End date"));

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
