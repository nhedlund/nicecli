using NiceCli.Tests.TestDomain;

namespace NiceCli.Tests.Scenarios;

public class GlobalOptionArgsTests
{
  [Test]
  [TestCase("-n")]
  [TestCase("--number")]
  [TestCase("--Number")]
  public void ArgsContainGlobalOptionNumberWithValue_NumberIsSet(string arg)
  {
    var app = CreateCliApp(arg, "22");

    app.Parse();

    var options = (MyGlobalOptions) app.Definition.Options.GlobalOptions;
    options.ShouldNotBeNull();
    options.Number.ShouldBe(22);
  }

  [Test]
  public async Task ArgsContainGlobalOptionNumberWithValue_CommandWithGlobalOptionsConstructorDependency_NumberIsSetFromConstructorDependency()
  {
    MyGlobalOptionsCommand.NumberAtExecute.ShouldBe(0);
    var app = CreateCliApp("--number", "22", "my-global-options");
    app.Parse();

    var result = await app.RunAsync();

    result.ShouldBe(0);
    app.SelectedCommand!.CommandType.ShouldBe(typeof(MyGlobalOptionsCommand));
    MyGlobalOptionsCommand.NumberAtExecute.ShouldBe(22);
    var options = (MyGlobalOptions) app.Definition.Options.GlobalOptions;
    options.ShouldNotBeNull();
    options.Number.ShouldBe(22);
  }

  [Test]
  public void ArgsContainGlobalOptionNumberWithValue_Parse_ConfigureIsCalledWithParsedGlobalOptions()
  {
    var numbersReceivedByConfigure = new List<int>();
    void ConfigureMethod(MyGlobalOptions o) => numbersReceivedByConfigure.Add(o.Number);
    var app = CreateCliAppWithConfigure(ConfigureMethod, "--number", "22");
    numbersReceivedByConfigure.ShouldBeEmpty();

    app.Parse();

    numbersReceivedByConfigure.Count.ShouldBe(1);
    numbersReceivedByConfigure.Single().ShouldBe(22);
  }

  private static CliApp CreateCliApp(params string[] args)
  {
    return CliApp.WithArgs(args)
      .DefaultCommand<MyDefaultRunCommand>("Runs the service")
      .Command<MyGlobalOptionsCommand>("Tests global options constructor dependency")
      .GlobalOptions<MyGlobalOptions>(c => c
        .Option(o => o.Number, "App number example", 'n'));
  }

  private static CliApp CreateCliAppWithConfigure(Action<MyGlobalOptions> configure, params string[] args)
  {
    return CliApp.WithArgs(args)
      .GlobalOptions<MyGlobalOptions>(c => c
        .Option(o => o.Number, "App number example", "value", 'n')
        .Configure(configure));
  }
}
