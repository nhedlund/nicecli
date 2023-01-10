using NiceCli.Core;
using NiceCli.Tests.TestDomain;

namespace NiceCli.Tests.Scenarios;

public class EnvironmentVariableFallbackTests
{
  [Test]
  [TestCase("", "VERBOSELOGGING")]
  [TestCase("", "VERBOSE_LOGGING")]
  [TestCase("", "VERBOSE__LOGGING")]
  [TestCase("TEST_", "TEST_VERBOSELOGGING")]
  [TestCase("TEST_", "TEST_VERBOSE_LOGGING")]
  [TestCase("TEST_", "TEST_VERBOSE__LOGGING")]
  public void GlobalOptionWithNoValue_EnvironmentVariableWithBooleanValue_EnvironmentVariableIsUsed(string prefix, string environmentVariable)
  {
    var app = CreateCliApp(prefix, environmentVariable, "");

    app.Parse();

    ((MyGlobalOptions) app.Definition.Options.GlobalOptions).VerboseLogging.ShouldBeTrue();
  }

  [Test]
  public void GlobalOptionWithNoValue_EnvironmentVariablesWithNoBooleanValue_EnvironmentVariableIsNotUsed()
  {
    var app = CreateCliApp("", "", "");

    app.Parse();

    ((MyGlobalOptions) app.Definition.Options.GlobalOptions).VerboseLogging.ShouldBeFalse();
  }

  [Test]
  public void GlobalOptionWithNoValue_EnvironmentVariableWithIncorrectPrefix_EnvironmentVariableIsNotUsed()
  {
    var app = CreateCliApp("WRONG_PREFIX", "VERBOSE_LOGGING", "");

    app.Parse();

    ((MyGlobalOptions) app.Definition.Options.GlobalOptions).VerboseLogging.ShouldBeFalse();
  }

  [Test]
  [TestCase("", "NUMBER")]
  [TestCase("TEST_", "TEST_NUMBER")]
  public void GlobalOptionWithNoValue_EnvironmentVariableWithIntegerValue_EnvironmentVariableIsUsed(string prefix, string environmentVariable)
  {
    var app = CreateCliApp(prefix, environmentVariable, "123");

    app.Parse();

    ((MyGlobalOptions) app.Definition.Options.GlobalOptions).Number.ShouldBe(123);
  }

  [Test]
  [TestCase("", "NUMBER")]
  [TestCase("TEST_", "TEST_NUMBER")]
  public void GlobalOptionWithValue_EnvironmentVariableWithIntegerValue_EnvironmentVariableIsNotUsed(string prefix, string environmentVariable)
  {
    var app = CreateCliApp(prefix, environmentVariable, "123", "--number", "555");

    app.Parse();

    ((MyGlobalOptions) app.Definition.Options.GlobalOptions).Number.ShouldBe(555);
  }

  private static CliApp CreateCliApp(string prefix, string environmentVariable, string environmentVariableValue, params string[] args)
  {
    var environmentVariables = !string.IsNullOrWhiteSpace(environmentVariable) ?
      new KeyValuePair<string, string>[] {new(environmentVariable, environmentVariableValue)} :
      Array.Empty<KeyValuePair<string, string>>();

    return CliApp.WithArgs(args)
      .AddGlobalParameterValueProvider(new EnvironmentVariableProvider(new TestEnvironmentVariableSource(environmentVariables), prefix))
      .DefaultCommand<MyDefaultRunCommand>("Runs the service")
      .GlobalOptions<MyGlobalOptions>(c => c
        .Flag(o => o.VerboseLogging, "Enable verbose logging", 'l')
        .Option(o => o.Number, "App number example", "value", 'n'));
  }

  private class TestEnvironmentVariableSource : IEnvironmentVariableSource
  {
    private readonly Dictionary<string, string> _environmentVariables;

    public TestEnvironmentVariableSource(params KeyValuePair<string ,string>[] environmentVariables)
    {
      _environmentVariables = environmentVariables.ToDictionary(kv => kv.Key, kv => kv.Value);
    }

    public Dictionary<string, string> GetEnvironmentVariables()
    {
      return _environmentVariables;
    }
  }
}
