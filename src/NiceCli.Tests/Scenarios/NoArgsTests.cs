using NiceCli.Commands;
using NiceCli.Tests.TestDomain;

namespace NiceCli.Tests.Scenarios;

public class NoArgsTests
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
  public void NoArgsAndNoDefaultCommand_Parse_DoesNotThrowAndHelpCommandIsSelected()
  {
    var app = CliApp.WithArgs();

    app.Parse();

    app.SelectedCommand.ShouldNotBeNull();
    app.SelectedCommand.CommandType.ShouldBe(typeof(CliHelpCommand));
  }

  [Test]
  public void NoArgsAndNonDefaultCommand_Parse_ThrowsException()
  {
    var app = CliApp.WithArgs()
      .Command<MyOtherCommand>("Does some stuff");

    app.Parse();
  }

  [Test]
  public void NoArgsAndDefaultCommand_Parse_DoesNotThrowExceptionAndCommandIsSelected()
  {
    var app = CliApp.WithArgs()
      .DefaultCommand<MyDefaultRunCommand>("Runs the service");

    app.Parse();

    app.SelectedCommand.ShouldNotBeNull();
    app.SelectedCommand.CommandType.ShouldBe(typeof(MyDefaultRunCommand));
  }

  [Test]
  public void NoArgsAndTwoCommandsWhereOneIsDefaultCommand_Parse_DoesNotThrowExceptionAndCommandIsSelected()
  {
    var app = CliApp.WithArgs()
      .Command<MyOtherCommand>("Does some stuff")
      .DefaultCommand<MyDefaultRunCommand>("Runs the service");

    app.Parse();

    app.SelectedCommand.ShouldNotBeNull();
    app.SelectedCommand.CommandType.ShouldBe(typeof(MyDefaultRunCommand));
  }
}
