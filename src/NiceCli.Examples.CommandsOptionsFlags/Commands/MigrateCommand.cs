namespace NiceCli.Examples.CommandsOptionsFlags.Commands;

public class MigrateCommand : ICliCommand
{
  private readonly IDatabaseOptions _database;

  public MigrateCommand(IDatabaseOptions database)
  {
    _database = database;
  }

  /// <summary>
  /// Show only the SQL statements for the migration, do not run them.
  /// </summary>
  public bool DryRun { get; set; }

  public Task ExecuteAsync()
  {
    Console.WriteLine("Migrate example...");
    Console.WriteLine($"Connection string: {_database.Db}");
    Console.WriteLine($"Command timeout: {_database.CommandTimeout}");
    Console.WriteLine($"Dry run: {DryRun}");
    return Task.CompletedTask;
  }
}
