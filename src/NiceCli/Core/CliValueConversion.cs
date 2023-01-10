namespace NiceCli.Core;

internal static class CliValueConversion
{
  public static long ToLong(string value) => long.Parse(value, CultureInfoIso.InvariantCultureWithIso8601);
  public static decimal ToDecimal(string value) => decimal.Parse(value.Replace(',', '.'), CultureInfoIso.InvariantCultureWithIso8601);
  public static DateTime ToDateTime(string value) => DateTime.Parse(value, CultureInfoIso.InvariantCultureWithIso8601);
}
