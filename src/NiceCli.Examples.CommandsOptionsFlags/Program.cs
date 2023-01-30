using NiceCli;
using NiceCli.Core;
using NiceCli.Examples.CommandsOptionsFlags;
using NiceCli.Examples.CommandsOptionsFlags.Commands;

// CLI app with some global options, one default command (that runs if no command is used) and a second non-default command.

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

static void InitializeLogging(ILoggingOptions loggingOptions)
{
  if (loggingOptions.Verbose)
    Console.WriteLine("Using verbose logging");
}
