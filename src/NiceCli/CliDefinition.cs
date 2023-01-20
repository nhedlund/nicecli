using System.Reflection;
using NiceCli.Core;

namespace NiceCli;

public class CliDefinition
{
  private CliSelectedCommand? _selectedCommand;

  internal string Name { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name ?? "";
  internal string Description { get; set; } = "";
  internal string Usage => $"{(Commands.HasDefaultCommand ? "[command]" : "<command>")} [args]";
  internal string AppVersion { get; set; } = Assembly.GetEntryAssembly()?
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "";
  internal List<string> Examples { get; } = new();
  internal List<string> LearnMore { get; } = new();

  internal CliCommands Commands { get; } = new();
  internal CliGlobalOptions Options { get; set; } = CliGlobalOptions.Create<object>();

  public CliSelectedCommand Parse(params string[] args)
  {
    if (_selectedCommand != null)
      return _selectedCommand;

    Commands.AddDefaultCommandsIfNotDefined();
    Options.AddMissingGlobalFlags();
    CliParameterValidator.ValidateDefinition(Options.Parameters, Commands);
    var selectedFromArgs = CliParameterParser.ParseParameters(args, Options.Parameters, Commands);
    _selectedCommand = new CliSelectedCommand(Options, Commands, selectedFromArgs);
    Options.AddFallbackProvidersGlobalParameterValues();
    Options.BindGlobalOptionValues();

    return _selectedCommand;
  }

  public IEnumerable<KeyValuePair<string, string>> GetGlobalParameterValues()
  {
    Parse();
    return Options.GetGlobalParameterValues();
  }

  internal void RegisterInternalDependencies(CliInternalContainer container)
  {
    container.AddSingleton(typeof(CliDefinition), this);

    var globalOptionsType = Options.GlobalOptions.GetType();
    container.AddSingleton(globalOptionsType, Options.GlobalOptions);

    var globalOptionsInterfaces = globalOptionsType.GetInterfaces()
      .Where(type => type.Namespace != null && !type.Namespace.StartsWith("System."));
    globalOptionsInterfaces.ForEach(type => container.AddSingleton(type, Options.GlobalOptions));

    Commands.ForEach(command => container.AddCommand(command.CommandType));
  }
}
