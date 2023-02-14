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
  public async Task UnknownArgsAndNoCommand_Parse_SucceedsAndRunReturns0(string args)
  {
    var app = CliApp.WithArgs(args.Split(' '));

    app.Parse();

    (await app.RunAsync()).ShouldBe(0);
  }

  [Test]
  [TestCase("-")]
  [TestCase("-u")]
  [TestCase("-u -a")]
  [TestCase("--")]
  [TestCase("--unknown")]
  [TestCase("unknown")]
  [TestCase("unknown yes")]
  public async Task UnknownArgsAndNoCommandAndThrowOnUnmappedParameters_Parse_ThrowsAndRunReturns1(string args)
  {
    var app = CliApp
      .WithArgs(args.Split(' '))
      .ThrowOnUnmappedParameters();

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
  public async Task UnknownArgsAndNonDefaultCommandAndThrowOnUnmappedParameters_Parse_DelaysExceptionAndRunReturns1(string args)
  {
    var app = CliApp.WithArgs(args.Split(' '))
      .Command<MyOtherCommand>("Does some stuff")
      .ThrowOnUnmappedParameters();

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
  public async Task UnknownArgsAndDefaultCommandAndThrowOnUnmappedParameters_Parse_DelaysExceptionAndRunReturns1(string args)
  {
    var app = CliApp.WithArgs(args.Split(' '))
      .DefaultCommand<MyDefaultRunCommand>("Runs the service")
      .ThrowOnUnmappedParameters();

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
  public async Task UnknownArgsAndTwoCommandsWhereOneIsDefaultCommandAndThrowOnUnmappedParameters_Parse_DelaysExceptionAndRunReturns1(string args)
  {
    var app = CliApp.WithArgs(args.Split(' '))
      .Command<MyOtherCommand>("Does some stuff")
      .DefaultCommand<MyDefaultRunCommand>("Runs the service")
      .ThrowOnUnmappedParameters();

    Should.Throw<CliUserException>(() => app.Parse());

    (await app.RunAsync()).ShouldBe(1);
  }
}
