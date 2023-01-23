using System.Globalization;

namespace NiceCli.Examples.CommandPositionalParameters.Commands;

public class GenerateCommand : ICliCommand
{
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public bool Weekday { get; set; }

  private int TotalDays => (int) (End - Start).TotalDays;

  public Task ExecuteAsync()
  {
    if (Start.Date > End.Date)
      throw new CliUserException("End date is before start date.");
    if (TotalDays > 10)
      throw new CliUserException("More than 10 days is not supported.");

    for (var i = 0; i <= TotalDays; i++)
    {
      var date = Start.Date.AddDays(i);
      Console.Write(date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

      if (Weekday)
        Console.Write($" {date.DayOfWeek}");

      Console.WriteLine();
    }

    return Task.CompletedTask;
  }
}
