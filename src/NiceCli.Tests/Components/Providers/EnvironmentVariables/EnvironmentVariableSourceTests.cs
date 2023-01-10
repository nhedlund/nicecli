using NiceCli.Core;

namespace NiceCli.Tests.Core;

public class EnvironmentVariableSourceTests
{
  [Test]
  public void GetEnvironmentVariables_ReturnsAtLeastTwoEnvironmentVariables()
  {
    var environmentVariableProvider = new EnvironmentVariableSource();

    var environmentVariables = environmentVariableProvider.GetEnvironmentVariables();

    environmentVariables.Count.ShouldBeGreaterThanOrEqualTo(2);
    environmentVariables.ShouldAllBe(environmentVariable => environmentVariable.Key.Length > 0);
    environmentVariables.ShouldContain(environmentVariable => environmentVariable.Value.Length > 0);
    environmentVariables.ShouldContain(environmentVariable => environmentVariable.Key != environmentVariable.Value);
  }
}
