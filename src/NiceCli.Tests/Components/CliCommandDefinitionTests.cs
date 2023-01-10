using NiceCli.Commands;
using NiceCli.Tests.TestDomain;

namespace NiceCli.Tests.Components;

public class CliCommandDefinitionTests
{
  [Test]
  public void Command_GetCommandName_ReturnsCorrectName()
  {
    var name = CliCommandDefinition.GetCommandName(typeof(MyCommandWithoutSuffix));

    name.ShouldBe("MyCommandWithoutSuffix");
  }

  [Test]
  public void CommandWithSuffix_GetCommandName_ReturnsCorrectName()
  {
    var name = CliCommandDefinition.GetCommandName(typeof(MyOtherCommand));

    name.ShouldBe("MyOther");
  }

  [Test]
  public void CommandWithPrefix_GetCommandName_ReturnsCorrectName()
  {
    var name = CliCommandDefinition.GetCommandName(typeof(CliMyCommandWithPrefix));

    name.ShouldBe("MyCommandWithPrefix");
  }

  [Test]
  public void CommandWithPrefixAndSuffix_GetCommandName_ReturnsCorrectName()
  {
    var name = CliCommandDefinition.GetCommandName(typeof(CliRunCommand));

    name.ShouldBe("Run");
  }
}
