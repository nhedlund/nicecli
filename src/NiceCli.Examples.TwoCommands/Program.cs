using NiceCli;
using NiceCli.Core;
using NiceCli.Examples.TwoCommands;
using NiceCli.Examples.TwoCommands.Commands;

// CLI app with some global options, one default command (that runs if no command is used) and a second non-default command.

return await CliApp.WithArgs(args)
  .GlobalOptions<GlobalOptions>(c => c
    .Option(o => o.Db, "Database connection string")
    .Option(o => o.CommandTimeout, "Database command timeout", "mm:ss", CliValueConversion.MinutesAndSecondsToTimeSpan)
    .Flag(o => o.Verbose, "Verbose logging")
    .Configure(InitializeLogging))
  .DefaultCommand<RunCommand>("Run service")
  .Command<MigrateCommand>("Migrate database")
  .RunAsync();

static void InitializeLogging(ILoggingOptions loggingOptions)
{
  if (loggingOptions.Verbose)
    Console.WriteLine($"Using verbose logging");
}
