using NiceCli.Commands;
using NiceCli.Core;

namespace NiceCli;

public class CliApp
{
  private readonly string[] _args;
  private Func<Task<int>>? _hostRun;
  private bool _parsed;

  private CliApp(params string[] args)
  {
    _args = args ?? throw new ArgumentNullException(nameof(args));
  }

  internal CliAppDefinition Definition { get; } = new();
  internal CliSelectedCommand? SelectedCommand { get; set; }
  internal CliInternalContainer Container { get; set; } = new();

  public static CliApp WithArgs(params string[] args)
  {
    return new CliApp(args);
  }

  public CliApp Parse()
  {
    if (_parsed)
      return this;

    SelectedCommand = Definition.Parse(_args);
    Definition.RegisterInternalDependencies(Container);
    Container.AddSingleton(typeof(CliSelectedCommand), SelectedCommand);
    Definition.Options.OnConfigured(Definition.Options.GlobalOptions);
    _parsed = true;
    return this;
  }

  public CliApp AddServiceProvider(IServiceProvider serviceProvider)
  {
    Container.AddExternalServiceProvider(serviceProvider);
    return this;
  }

  public CliApp RegisterHostRun(Func<Task<int>> hostRun)
  {
    _hostRun = hostRun;
    return this;
  }

  public IEnumerable<KeyValuePair<string, string>> GetGlobalParameterValues()
  {
    Parse();
    return Definition.GetGlobalParameterValues();
  }

  public object GetGlobalOptions()
  {
    Parse();
    return Definition.Options.GlobalOptions;
  }

  public async Task<int> RunAsync()
  {
    try
    {
      Parse();

      if (SelectedCommand == null)
        throw new InvalidOperationException($"{nameof(SelectedCommand)} is null.");

      return SelectedCommand.CommandType == typeof(CliRunCommand) ?
        await RunHostAsync() :
        await RunSelectedCommandAsync();
    }
    catch (CliUserException exception)
    {
      await Console.Error.WriteLineAsync(exception.Message);
      await Console.Error.WriteLineAsync();
      await Console.Error.WriteLineAsync($"Show help with --{BuiltInFlags.HelpName.ToLowerInvariant()}");
      return 1;
    }
  }

  private async Task<int> RunHostAsync()
  {
    if (_hostRun == null)
      throw new InvalidOperationException($"To run the host you must call {nameof(RegisterHostRun)} first with the func to execute.");

    return await _hostRun.Invoke();
  }

  private async Task<int> RunSelectedCommandAsync()
  {
    var commandInstance = Container.ResolveCommand(SelectedCommand!.CommandType);
    SelectedCommand.BindCommand(commandInstance);

    await commandInstance.ExecuteAsync();
    return 0;
  }
}
