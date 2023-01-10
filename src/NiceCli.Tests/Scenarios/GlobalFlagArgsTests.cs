using NiceCli.Commands;
using NiceCli.Tests.TestDomain;

namespace NiceCli.Tests.Scenarios;

public class GlobalFlagArgsTests
{
  [Test]
  [TestCase("-h")]
  [TestCase("--help")]
  [TestCase("--Help")]
  public async Task HelpWithDefaultGlobalConfigurationAndNoSelectedCommand_HelpIsSetAndRunReturns0(string arg)
  {
    var app = CliApp.WithArgs(arg)
      .Command<MyOtherCommand>("Run other command")
      .Parse();

    app.Definition.Options.GlobalOptions.ShouldNotBeNull();
    app.Definition.Options.IsHelpRequested.ShouldBeTrue();
    app.SelectedCommand.ShouldNotBeNull();
    app.SelectedCommand.CommandType.ShouldBe(typeof(CliHelpCommand));
    (await app.RunAsync()).ShouldBe(0);
  }

  [Test]
  [TestCase("-v")]
  [TestCase("--version")]
  [TestCase("--Version")]
  public async Task VersionWithDefaultGlobalConfigurationAndNoSelectedCommand_VersionIsSetAndRunReturns0(string arg)
  {
    var app = CliApp.WithArgs(arg)
      .Command<MyOtherCommand>("Run other command")
      .Parse();

    app.Definition.Options.GlobalOptions.ShouldNotBeNull();
    app.Definition.Options.IsVersionRequested.ShouldBeTrue();
    app.SelectedCommand.ShouldNotBeNull();
    app.SelectedCommand.CommandType.ShouldBe(typeof(CliVersionCommand));
    (await app.RunAsync()).ShouldBe(0);
  }

  [Test]
  [TestCase("-b", false)]
  [TestCase("--verbose-logging", false)]
  [TestCase("--Verbose-logging", false)]
  [TestCase("-b", true)]
  [TestCase("--verbose-logging", true)]
  [TestCase("--Verbose-logging", true)]
  public void VerboseWithCustomGlobalConfigurationAndSelectedCommand_VerboseIsSet(string arg, bool globalsFirst)
  {
    var arguments = globalsFirst ? new[] {"my-other", arg} : new[] {arg, "my-other"};

    var app = CliApp.WithArgs(arguments)
      .Command<MyOtherCommand>("Run other command")
      .GlobalOptions<MyGlobalOptions>(g => g.Flag(f => f.VerboseLogging, "Verbose logging", 'b'))
      .Parse();

    app.Definition.Options.GlobalOptions.ShouldNotBeNull();
    ((MyGlobalOptions) app.Definition.Options.GlobalOptions).VerboseLogging.ShouldBeTrue();
  }

  [Test]
  [TestCase("-b", false)]
  [TestCase("--verbose-logging", false)]
  [TestCase("--Verbose-logging", false)]
  [TestCase("-b", true)]
  [TestCase("--verbose-logging", true)]
  [TestCase("--Verbose-logging", true)]
  public void HiddenVerboseWithCustomGlobalConfigurationAndSelectedCommand_VerboseIsSet(string arg, bool globalsFirst)
  {
    var arguments = globalsFirst ? new[] {"my-other", arg} : new[] {arg, "my-other"};

    var app = CliApp.WithArgs(arguments)
      .Command<MyOtherCommand>("Run other command")
      .GlobalOptions<MyGlobalOptions>(g => g.HiddenFlag(f => f.VerboseLogging, "Verbose logging", 'b'))
      .Parse();

    app.Definition.Options.GlobalOptions.ShouldNotBeNull();
    ((MyGlobalOptions) app.Definition.Options.GlobalOptions).VerboseLogging.ShouldBeTrue();
  }

  [Test]
  [TestCase("-v")]
  [TestCase("--version")]
  [TestCase("--Version")]
  public void VersionWithCommandWithTheSameLongOrShortParameterName_ParseThrows(string arg)
  {
    Should.Throw<InvalidOperationException>(() => CliApp.WithArgs()
      .DefaultCommand<MyDefaultRunCommand>("Runs the service",
        c => c.Flag(f => f.Verbose, "Enable verbose logging", 'v'))
      .Parse());
  }

  [Test]
  public void NonUniqueParameterNames_ParseThrows()
  {
    Should.Throw<InvalidOperationException>(() => CliApp.WithArgs()
      .DefaultCommand<MyDefaultRunCommand>("Runs the service")
      .GlobalOptions<MyGlobalOptions>(c => c
        .Flag(f => f.VerboseLogging, "Some flag", 'x')
        .Flag(f => f.VerboseLogging, "Some flag", 'x'))
      .Parse());
  }
}
