namespace NiceCli.Examples.TwoCommands;

public interface IDatabaseOptions
{
  /// <summary>
  /// Database connection string.
  /// </summary>
  string Db { get; }

  /// <summary>
  /// Database command timeout.
  /// </summary>
  TimeSpan CommandTimeout { get; }
}
