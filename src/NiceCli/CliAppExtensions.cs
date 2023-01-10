using NiceCli.Commands;
using NiceCli.Core;

namespace NiceCli;

public static class CliAppExtensions
{
  public static CliApp Named(this CliApp app, string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException($"{nameof(name)} is null or empty.");

    app.Definition.Name = name;
    return app;
  }

  public static CliApp Description(this CliApp app, string description)
  {
    if (string.IsNullOrWhiteSpace(description))
      throw new ArgumentException($"{nameof(description)} is null or empty.");

    app.Definition.Description = description;
    return app;
  }

  public static CliApp Usage(this CliApp app, string usage)
  {
    if (string.IsNullOrWhiteSpace(usage))
      throw new ArgumentException($"{nameof(usage)} is null or empty.");

    app.Definition.Usage = usage;
    return app;
  }

  public static CliApp Version(this CliApp app, string version)
  {
    if (string.IsNullOrWhiteSpace(version))
      throw new ArgumentException($"{nameof(version)} is null or empty.");

    app.Definition.AppVersion = version;
    return app;
  }

  public static CliApp GlobalOptions<TGlobalOptions>(
    this CliApp app,
    Action<CliGlobalOptions<TGlobalOptions>> configure) where TGlobalOptions : class, new()
  {
    if (app.Definition.Options.GlobalOptions.GetType() != typeof(TGlobalOptions))
      app.Definition.Options = app.Definition.Options.CopyWithNewOptionsInstance(new TGlobalOptions());
    else
      throw new InvalidOperationException($"{nameof(GlobalOptions)} can only be configured once.");

    configure((CliGlobalOptions<TGlobalOptions>) app.Definition.Options);
    return app;
  }

  public static CliApp Example(this CliApp app, string example)
  {
    if (string.IsNullOrWhiteSpace(example))
      throw new ArgumentException($"{nameof(example)} is null or empty.");

    app.Definition.Examples.Add(example);
    return app;
  }

  public static CliApp LearnMore(this CliApp app, string learnMore)
  {
    if (string.IsNullOrWhiteSpace(learnMore))
      throw new ArgumentException($"{nameof(learnMore)} is null or empty.");

    app.Definition.LearnMore.Add(learnMore);
    return app;
  }

  public static CliApp CommandRun(this CliApp app, string description, CliDefault isDefault)
  {
    if (app.Definition.Commands.TryGetCommandDefinitionImplementing<CliRunCommand>() == null)
      app.Command<CliRunCommand>(description, isDefault, CliVisibility.Visible);

    return app;
  }

  public static CliApp Command<TCommand>(
    this CliApp app,
    string description,
    Action<CliCommandDefinition<TCommand>>? configureCommand = null) where TCommand : ICliCommand
  {
    return app.Command<TCommand>(description, CliDefault.No, CliVisibility.Visible, configureCommand);
  }

  public static CliApp HiddenCommand<TCommand>(
    this CliApp app,
    string description,
    Action<CliCommandDefinition<TCommand>>? configureCommand = null) where TCommand : ICliCommand
  {
    return app.Command<TCommand>(description, CliDefault.No, CliVisibility.Hidden, configureCommand);
  }

  public static CliApp DefaultCommand<TCommand>(
    this CliApp app,
    string description,
    Action<CliCommandDefinition<TCommand>>? configureCommand = null) where TCommand : ICliCommand
  {
    return app.Command<TCommand>(description, CliDefault.Yes, CliVisibility.Visible, configureCommand);
  }

  private static CliApp Command<TCommand>(
    this CliApp app,
    string description,
    CliDefault defaultCommand,
    CliVisibility visibility,
    Action<CliCommandDefinition<TCommand>>? configureCommand = null) where TCommand : ICliCommand
  {
    var command = new CliCommandDefinition<TCommand>(description, typeof(TCommand), defaultCommand, visibility);
    configureCommand?.Invoke(command);
    app.Definition.Commands.Add(command);
    return app;
  }

  public static CliApp AddEnvironmentVariablesPrefixed(this CliApp app, string prefix)
  {
    return app.AddGlobalParameterValueProvider(new EnvironmentVariableProvider(new EnvironmentVariableSource(), prefix));
  }

  public static CliApp AddGlobalParameterValueProvider(this CliApp app, IGlobalParameterValueProvider globalParameterValueProvider)
  {
    app.Definition.Options.AddGlobalParameterValueProvider(globalParameterValueProvider);
    return app;
  }
}
