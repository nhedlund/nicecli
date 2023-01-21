using NiceCli.Core;
using NiceCli.Tests.TestDomain;

namespace NiceCli.Tests.Components;

public class CliDefinitionTests
{
  [Test]
  public void GlobalOptionsWithTwoInterfaces_RegisterInternalDependencies_RegistersTheClassAndTheTwoInterfacesInTheContainer()
  {
    var container = new CliInternalContainer();
    var definition = new CliAppDefinition {Options = CliGlobalOptions.Create<MyGlobalOptions>()};
    container.AddCommand(typeof(MyTestCommand));

    definition.RegisterInternalDependencies(container);

    var command = (MyTestCommand) container.ResolveCommand(typeof(MyTestCommand));
    command.ShouldNotBeNull();
    command.MyGlobalOptions.ShouldNotBeNull();
    command.MyGlobalOptions.ShouldBe(definition.Options.GlobalOptions);
    command.MyLoggingOptions.ShouldBe(command.MyGlobalOptions);
    command.MyNumberOptions.ShouldBe(command.MyGlobalOptions);
  }

  private class MyTestCommand : ICliCommand
  {
    public MyTestCommand(MyGlobalOptions myGlobalOptions, IMyLoggingOptions myLoggingOptions, IMyNumberOptions myNumberOptions)
    {
      MyGlobalOptions = myGlobalOptions;
      MyLoggingOptions = myLoggingOptions;
      MyNumberOptions = myNumberOptions;
    }

    public MyGlobalOptions MyGlobalOptions { get; }
    public IMyLoggingOptions MyLoggingOptions { get; }
    public IMyNumberOptions MyNumberOptions { get; }

    public Task ExecuteAsync()
    {
      return Task.CompletedTask;
    }
  }
}
