using System.Globalization;

namespace NiceCli.Core;

internal static class CultureInfoIso
{
  /// <summary>A read-only clone of <see cref="System.Globalization.CultureInfo.InvariantCulture" />,
  /// except the <see cref="System.Globalization.CultureInfo.DateTimeFormat" /> is replaced with a
  /// <see cref="System.Globalization.DateTimeFormatInfo" /> configured to use ISO 8601 formatting for dates and times.
  /// </summary>
  /// <remarks>From: https://stackoverflow.com/a/12447289</remarks>
  public static CultureInfo InvariantCultureWithIso8601 { get; } = CreateInvariantCultureWithIso8601();

  private static CultureInfo CreateInvariantCultureWithIso8601()
  {
    var cultureInfo = (CultureInfo) CultureInfo.InvariantCulture.Clone();
    cultureInfo.DateTimeFormat = new DateTimeFormatInfo
    {
      AMDesignator = "AM",
      DateSeparator = "-",
      FirstDayOfWeek = DayOfWeek.Monday,
      CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek,
      FullDateTimePattern = "yyyy-MM-dd HH':'mm':'ss", // NOTE: Use `"yyyy-MM-DD'T'HH:mm:ss"` for stricter ISO 8601 compliance.
      LongDatePattern = "yyyy-MM-dd ddd",              // <-- This is subjective.
      LongTimePattern = "HH':'mm':'ss",
      MonthDayPattern = "MMMM dd",
      PMDesignator = "PM",
      ShortDatePattern = "yyyy-MM-dd",
      ShortTimePattern = "HH:mm",
      TimeSeparator = ":",
      YearMonthPattern = "yyyy MMMM"
    };
    return CultureInfo.ReadOnly(cultureInfo);
  }

  public static string ToStringInvariant(this DateTime value)
  {
    return value.ToString(provider: InvariantCultureWithIso8601);
  }

  public static string ToStringInvariant(this DateTime value, string format)
  {
    return value.ToString(format: format, provider: InvariantCultureWithIso8601);
  }
}
