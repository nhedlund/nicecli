using NiceCli.Tests.TestDomain;

namespace NiceCli.Tests.Scenarios;

public class UnknownArgsTests
{
  [Test]
  public void NoArgsAndDefaultCommand_Parse_HelpAndVersionIsNotSet()
  {
    var app = CliApp.WithArgs()
      .DefaultCommand<MyDefaultRunCommand>("Runs the service").Parse();

    app.Definition.Options.GlobalOptions.ShouldNotBeNull();
    app.Definition.Options.IsHelpRequested.ShouldBeFalse();
    app.Definition.Options.IsVersionRequested.ShouldBeFalse();
  }

  [Test]
  [TestCase("-")]
  [TestCase("-u")]
  [TestCase("-u -a")]
  [TestCase("--")]
  [TestCase("--unknown")]
  [TestCase("unknown")]
  [TestCase("unknown yes")]
  public async Task UnknownArgsAndNoCommand_Parse_DelaysExceptionAndRunReturns1(string args)
  {
    var app = CliApp.WithArgs(args.Split(' '));

    Should.Throw<CliUserException>(() => app.Parse());

    (await app.RunAsync()).ShouldBe(1);
  }

  [Test]
  [TestCase("-")]
  [TestCase("-u")]
  [TestCase("-u -a")]
  [TestCase("--")]
  [TestCase("--unknown")]
  [TestCase("unknown")]
  [TestCase("unknown yes")]
  public async Task UnknownArgsAndNonDefaultCommand_Parse_DelaysExceptionAndRunReturns1(string args)
  {
    var app = CliApp.WithArgs(args.Split(' '))
      .Command<MyOtherCommand>("Does some stuff");

    Should.Throw<CliUserException>(() => app.Parse());

    (await app.RunAsync()).ShouldBe(1);
  }

  [Test]
  [TestCase("-")]
  [TestCase("-u")]
  [TestCase("-u -a")]
  [TestCase("--")]
  [TestCase("--unknown")]
  [TestCase("unknown")]
  [TestCase("unknown yes")]
  public async Task UnknownArgsAndDefaultCommand_Parse_DelaysExceptionAndRunReturns1(string args)
  {
    var app = CliApp.WithArgs(args.Split(' '))
      .DefaultCommand<MyDefaultRunCommand>("Runs the service");

    Should.Throw<CliUserException>(() => app.Parse());

    (await app.RunAsync()).ShouldBe(1);
  }

  [Test]
  [TestCase("-")]
  [TestCase("-u")]
  [TestCase("-u -a")]
  [TestCase("--")]
  [TestCase("--unknown")]
  [TestCase("unknown")]
  [TestCase("unknown yes")]
  public async Task UnknownArgsAndTwoCommandsWhereOneIsDefaultCommand_Parse_DelaysExceptionAndRunReturns1(string args)
  {
    var app = CliApp.WithArgs(args.Split(' '))
      .Command<MyOtherCommand>("Does some stuff")
      .DefaultCommand<MyDefaultRunCommand>("Runs the service");

    Should.Throw<CliUserException>(() => app.Parse());

    (await app.RunAsync()).ShouldBe(1);
  }
}
