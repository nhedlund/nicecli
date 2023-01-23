using NiceCli.Core;

namespace NiceCli;

public class CliGlobalOptions
{
  private readonly List<IGlobalParameterValueProvider> _globalParameterValueProviders;

  protected CliGlobalOptions(
    List<IGlobalParameterValueProvider> globalParameterValueProviders,
    List<CliParameter> parameters,
    object globalOptions)
  {
    _globalParameterValueProviders = globalParameterValueProviders;
    Parameters = parameters;
    GlobalOptions = globalOptions;
  }

  internal List<CliParameter> Parameters { get; }
  internal object GlobalOptions { get; }
  internal Action<object> OnConfigured { get; set; } = _ => { };

  internal IEnumerable<CliOption> Options => Parameters.OfType<CliOption>();
  internal IEnumerable<CliFlag> Flags => Parameters.OfType<CliFlag>();

  internal bool IsHelpRequested => Flags.Any(flag => flag.IsHelpRequested());
  internal bool IsVersionRequested => Flags.Any(flag => flag.Name == BuiltInFlags.VersionName && flag.HasValue());

  public void AddGlobalParameterValueProvider(IGlobalParameterValueProvider globalParameterValueProvider)
  {
    _globalParameterValueProviders.Add(globalParameterValueProvider);
  }

  internal void AddMissingGlobalFlags()
  {
    if (!HasFlag(BuiltInFlags.HelpName))
      AddInternalGlobalFlag(BuiltInFlags.HelpName, "Show help for command", BuiltInFlags.HelpShortName);

    if (!HasFlag(BuiltInFlags.VersionName))
      AddInternalGlobalFlag(BuiltInFlags.VersionName, "Show version", BuiltInFlags.VersionShortName);
  }

  internal void AddFallbackProvidersGlobalParameterValues()
  {
    foreach (var parameter in Parameters)
    {
      foreach (var globalParameterValueProvider in _globalParameterValueProviders)
      {
        if (parameter.HasValue())
          break;

        parameter.Value = globalParameterValueProvider.GetParameterValue(parameter.Name);
      }
    }
  }

  internal void BindGlobalOptionValues()
  {
    Parameters.ForEach(parameter => parameter.MapValueToObjectIfSet(GlobalOptions));
  }

  internal IEnumerable<KeyValuePair<string, string>> GetGlobalParameterValues()
  {
    return Parameters
      .Where(parameter => parameter.Value != null)
      .OrderBy(parameter => parameter.Name)
      .Select(parameter => new KeyValuePair<string, string>(parameter.Name, parameter.Value!));
  }

  internal static CliGlobalOptions<TGlobalOptions> Create<TGlobalOptions>() where TGlobalOptions : class, new()
  {
    return new CliGlobalOptions<TGlobalOptions>(new List<IGlobalParameterValueProvider>(), new List<CliParameter>(), new TGlobalOptions());
  }

  internal CliGlobalOptions<TGlobalOptions> CopyWithNewOptionsInstance<TGlobalOptions>(TGlobalOptions globalOptions) where TGlobalOptions : class, new()
  {
    return new CliGlobalOptions<TGlobalOptions>(_globalParameterValueProviders, Parameters, globalOptions);
  }

  private void AddInternalGlobalFlag(string name, string description, char shortName)
  {
    var flag = new CliFlag(
      name,
      shortName,
      name.PascalToKebabCase(),
      description,
      CliVisibility.Visible,
      _ => { });

    Parameters.Add(flag);
  }

  private bool HasFlag(string longName)
  {
    return Flags.Any(flag => flag.MatchingNames.Any(name =>
      name.Equals($"{CliParameter.LongNamePrefix}{longName}", StringComparison.OrdinalIgnoreCase)));
  }
}

public class CliGlobalOptions<TGlobalOptions> : CliGlobalOptions
  where TGlobalOptions : class, new()
{
  public CliGlobalOptions(
    List<IGlobalParameterValueProvider> globalParameterValueProviders,
    List<CliParameter> parameters,
    TGlobalOptions globalOptions) : base(globalParameterValueProviders, parameters, globalOptions)
  {
  }
}
