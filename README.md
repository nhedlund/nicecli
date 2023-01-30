# NiceCli
[![CI](https://github.com/nhedlund/nicecli/actions/workflows/ci.yml/badge.svg)](https://github.com/nhedlund/nicecli/actions/workflows/ci.yml)

NiceCli is a command-based CLI framework for .NET applications.

Its main design goal is to keep the definition of the global options, commands and their options in one place using fluent syntax, and the commands themselves as separate classes.

By using convention over configuration the CLI definition can be kept clean and minimal. For example it is not necessary to define parameter names, the property names are used like for example CommandTimeout is mapped to "--command-timeout".

## Quick start

Add the Nuget package **NiceCli** by using your IDE or through the terminal:

```bash
dotnet add package NiceCli
```

Define the CLI and add classes for commands:

* Start with `CliApp.WithArgs(args)`
* Add an optional global options class to any hold global options and flags with `GlobalOptions<Type>`, where Type is the global options class
* Add one or more command definitions with `Command<Type>`, `DefaultCommand<Type>` or `HiddenCommand<Type>` where Type is the command class
* Add one or more command classes that inherit the interface ICliCommand together with optional properties for command options and flags
* End with `RunAsync` for a simple app that do not use IoC, or add the Nuget package **NiceCli.Dotnet** to use the .NET IoC framework

## Examples

### Global options, commands, options and flags

CLI app with some global options, one default command (that runs if no command is used) and a second non-default command with a flag.

Logging is setup early in the method InitializeLogging that receives GlobalOptions with the Verbose property as a parameter. It is called by the Configure method which is an optional extension for global options.

Not shown in this example is the optional char parameter in Option and Flag definitions to add shortcut parameters like for example "-h" for help.

Program.cs:

```csharp
using NiceCli;

return await CliApp.WithArgs(args)
  .GlobalOptions<GlobalOptions>(c => c
    .Option(o => o.Db, "Database connection string")
    .Option(o => o.CommandTimeout, "Database command timeout", "mm:ss", CliValueConversion.MinutesAndSecondsToTimeSpan)
    .Flag(f => f.Verbose, "Verbose logging")
    .Configure(o => InitializeLogging(o)))
  .DefaultCommand<RunCommand>("Run service")
  .Command<MigrateCommand>("Migrate database to current version", c => c
    .Flag(f => f.DryRun, "Show migration SQL commands, but do not run them"))
  .RunAsync();
```

Help output for the above code when running: `example -h`

<img alt="Commands options and flags example" width="827px" src="https://raw.githubusercontent.com/nhedlund/nicecli/main/docs/images/commands-options-flags.png" />

And help output for the command migrate: `example migrate -h`

<img alt="Migrate command help" width="827px" src="https://raw.githubusercontent.com/nhedlund/nicecli/main/docs/images/commands-options-flags-migrate.png" />

### Descriptive definitions

CLI app with overridden name and version, description, examples and learn more info.

Program.cs:

```csharp
using NiceCli;

return await CliApp.WithArgs(args)
  .Named("custom-name")
  .Description("This app only serves as an example")
  .Version("1.0-custom-version")
  .Example("custom-name run")
  .Example("custom-name migrate")
  .Example("custom-name export <start-date> <end-date>")
  .LearnMore("Read about defining the CLI at https://github.com/nhedlund/nicecli")
  .RunAsync();
```

Help output for the above code when running: `example -h`

<img alt="Descriptive parameters example" width="827px" src="https://raw.githubusercontent.com/nhedlund/nicecli/main/docs/images/cli-description.png" />

### Positional parameters

CLI app with one command that has two positional parameters and a flag.

Program.cs:

```csharp
using NiceCli;

return await CliApp.WithArgs(args)
  .Command<GenerateCommand>("Generate a list of dates from start date to end date", c => c
    .PositionalParameter(p => p.Start, "Start date: yyyy-mm-dd")
    .PositionalParameter(p => p.End, "End date: yyyy-mm-dd")
    .Flag(f => f.Weekday, "Include weekday name"))
  .RunAsync();
```

Help output for the above code when running: `example -h`

<img alt="Positional parameters example" width="827px" src="https://raw.githubusercontent.com/nhedlund/nicecli/main/docs/images/command-positional-parameters.png" />

And help output for the command generate: `example generate -h`

<img alt="Generate command help" width="827px" src="https://raw.githubusercontent.com/nhedlund/nicecli/main/docs/images/command-positional-parameters-generate.png" />

### Dotnet host framework integration

CLI app has one default command that runs the host. The run command is not a normal command, it is a built-in command that starts the host.

The run command definition takes two arguments: description and whether the command should run by default when no command parameter is used, or if a command parameter is necessary.

Program.cs:

```csharp
using NiceCli;
using NiceCli.Dotnet;

var cliApp = CliApp.WithArgs(args)
  .CommandRun("Start example service", CliDefault.Yes);

using var host = Host.CreateDefaultBuilder()
  .ConfigureCli(cliApp)
  .Build();

return await host.RunCliCommandAsync(cliApp);
```

Help output for the above code when running: `example -h`

<img alt="Dotnet run host help" width="827px" src="https://raw.githubusercontent.com/nhedlund/nicecli/main/docs/images/dotnet-run-host.png" />
