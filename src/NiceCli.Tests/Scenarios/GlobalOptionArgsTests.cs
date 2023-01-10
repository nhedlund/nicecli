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

  private static CliApp CreateCliApp(params string[] args)
  {
    return CliApp.WithArgs(args)
      .DefaultCommand<MyDefaultRunCommand>("Runs the service")
      .Command<MyGlobalOptionsCommand>("Tests global options constructor dependency")
      .GlobalOptions<MyGlobalOptions>(c => c
        .Option(o => o.Number, "App number example", "value", 'n'));
  }
}
