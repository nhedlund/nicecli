using NiceCli;
using NiceCli.Examples.CommandPositionalParameters.Commands;

// CLI app with one command that has two positional parameters and a flag.

return await CliApp.WithArgs(args)
  .Command<GenerateCommand>("Generate a list of dates from start date to end date", c => c
    .PositionalParameter(p => p.Start, "Start date: yyyy-mm-dd")
    .PositionalParameter(p => p.End, "End date: yyyy-mm-dd")
    .Flag(f => f.Weekday, "Include weekday name"))
  .RunAsync();
