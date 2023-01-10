using Microsoft.Extensions.Configuration;
using NiceCli.Dotnet;
using NiceCli.Tests.TestDomain;

namespace NiceCli.Tests.Scenarios;

public class ConfigurationTests
{
  private static readonly string[] Args = {"-a", "--beta", "--gamma-option", "gammaOptionValue"};

  [Test]
  public void DotnetCliArgumentCommandLineParsing_Parse_ParsedWithoutFlagSupportAndNoStrippingOfDashes()
  {
    var configuration = new ConfigurationBuilder()
      .AddCommandLine(Args, new Dictionary<string, string> { {"-a", "alpha"}})
      .Build();

    var config = configuration.AsEnumerable().ToList();

    config.Count.ShouldBe(2);
    config[0].ShouldBe(new KeyValuePair<string, string>("gamma-option", "gammaOptionValue"));
    config[1].ShouldBe(new KeyValuePair<string, string>("alpha", "--beta"));
  }

  [Test]
  public void CliArgumentCommandLineParsing_Parse_CorrectlyParsedAndCaseInsensitive()
  {
    var app = CreateCliApp(Args);
    var configuration = new ConfigurationBuilder()
      .AddCli(app)
      .Build();

    var config = configuration.AsEnumerable().ToList();

    config.Count.ShouldBe(3);
    config[0].ShouldBe(new KeyValuePair<string, string>("GammaOption", "gammaOptionValue"));
    config[1].ShouldBe(new KeyValuePair<string, string>("Beta", "True"));
    config[2].ShouldBe(new KeyValuePair<string, string>("Alpha", "True"));
  }

  private static CliApp CreateCliApp(params string[] args)
  {
    return CliApp.WithArgs(args)
      .DefaultCommand<MyDefaultRunCommand>("Runs the service")
      .GlobalOptions<ConfigurationTestsGlobalOptions>(c => c
        .Flag(f => f.Alpha, "Alpha flag", 'a')
        .Flag(f => f.Beta, "Beta flag", 'b')
        .Option(o => o.GammaOption, "Gamma option", "value"));
  }

  private class ConfigurationTestsGlobalOptions
  {
    public bool Alpha { get; set; }
    public bool Beta { get; set; }
    public string GammaOption { get; set; } = "";
  }
}
