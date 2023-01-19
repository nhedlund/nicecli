using System.Text.RegularExpressions;

namespace NiceCli.Core;

public static class CliValueConversion
{
  private static readonly Regex MonthAndDayWithoutSeparatorRegex = new(@"^\d{4}$");
  private static readonly Regex YearAndMonthAndDayWithoutSeparatorRegex = new(@"^\d{6}$");

  public static long ToLong(string value) => long.Parse(value, CultureInfoIso.InvariantCultureWithIso8601);
  public static double ToDouble(string value) => double.Parse(value.Replace(',', '.'), CultureInfoIso.InvariantCultureWithIso8601);
  public static decimal ToDecimal(string value) => decimal.Parse(value.Replace(',', '.'), CultureInfoIso.InvariantCultureWithIso8601);
  public static TimeSpan HoursAndMinutesToTimeSpan(string value) => TimeSpan.Parse(value, CultureInfoIso.InvariantCultureWithIso8601);
  public static TimeSpan MinutesAndSecondsToTimeSpan(string value) => TimeSpan.ParseExact(value, @"m\:ss", CultureInfoIso.InvariantCultureWithIso8601);

  public static DateTime ToDateTime(string value)
  {
    if (value.Length == 4 && MonthAndDayWithoutSeparatorRegex.IsMatch(value))
      value = $"{value.Substring(0, 2)}-{value.Substring(2, 2)}";
    else if (value.Length == 6 && YearAndMonthAndDayWithoutSeparatorRegex.IsMatch(value))
      value = $"{value.Substring(0, 2)}-{value.Substring(2, 2)}-{value.Substring(4, 2)}";

    return DateTime.Parse(value, CultureInfoIso.InvariantCultureWithIso8601);
  }
}
