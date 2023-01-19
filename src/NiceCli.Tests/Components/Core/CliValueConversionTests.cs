using NiceCli.Core;

namespace NiceCli.Tests.Components.Core;

public class CliValueConversionTests
{
  [Test]
  [TestCase("0", 0)]
  [TestCase("1", 1)]
  [TestCase("-1", -1)]
  [TestCase("22", 22)]
  [TestCase("-22", -22)]
  [TestCase("100200300400500", 100200300400500)]
  public void String_ToLong_ConvertsCorrect(string input, long expected)
  {
    var value = CliValueConversion.ToLong(input);

    value.ShouldBe(expected);
  }

  [Test]
  [TestCase("")]
  [TestCase("a")]
  [TestCase("1.0")]
  [TestCase("1.1")]
  [TestCase("1,1")]
  [TestCase("-1.1")]
  public void InvalidString_ToLong_ThrowsException(string input)
  {
    Should.Throw<FormatException>(() => CliValueConversion.ToLong(input));
  }

  [Test]
  [TestCase("22", 22)]
  [TestCase("0", 0)]
  [TestCase("0.00004", 0.00004)]
  [TestCase("-0.00004", -0.00004)]
  [TestCase("1", 1)]
  [TestCase("-1", -1)]
  [TestCase("22", 22)]
  [TestCase("-22", -22)]
  [TestCase("22.5", 22.5)]
  [TestCase("22,5", 22.5)]
  [TestCase("-22.5", -22.5)]
  [TestCase("100200300400500", 100200300400500.0)]
  public void String_ToDecimal_ConvertsCorrect(string input, decimal expected)
  {
    var value = CliValueConversion.ToDecimal(input);

    value.ShouldBe(expected);
  }

  [Test]
  [TestCase("")]
  [TestCase("a")]
  public void InvalidString_ToDecimal_ThrowsException(string input)
  {
    Should.Throw<FormatException>(() => CliValueConversion.ToDecimal(input));
  }

  [Test]
  [TestCase("22", 22)]
  [TestCase("0", 0)]
  [TestCase("0.00004", 0.00004)]
  [TestCase("-0.00004", -0.00004)]
  [TestCase("1", 1)]
  [TestCase("-1", -1)]
  [TestCase("22", 22)]
  [TestCase("-22", -22)]
  [TestCase("22.5", 22.5)]
  [TestCase("22,5", 22.5)]
  [TestCase("-22.5", -22.5)]
  [TestCase("100200300400500", 100200300400500.0)]
  public void String_ToDouble_ConvertsCorrect(string input, double expected)
  {
    var value = CliValueConversion.ToDouble(input);

    value.ShouldBe(expected);
  }

  [Test]
  [TestCase("")]
  [TestCase("a")]
  public void InvalidString_ToDouble_ThrowsException(string input)
  {
    Should.Throw<FormatException>(() => CliValueConversion.ToDouble(input));
  }

  [Test]
  [TestCase("2025-01-15", 2025, 01, 15)]
  [TestCase("250115", 2025, 01, 15)]
  [TestCase("1999-12-01", 1999, 12, 01)]
  [TestCase("99-12-01", 1999, 12, 01)]
  [TestCase("99-12-01", 1999, 12, 01)]
  public void String_ToDateTime_ConvertsCorrect(string input, int year, int month, int day)
  {
    var expected = new DateTime(year, month, day);

    var value = CliValueConversion.ToDateTime(input);

    value.ShouldBe(expected);
  }

  [Test]
  [TestCase("10-11")]
  [TestCase("1011")]
  public void StringWithMonthAndDay_ToDateTime_ConvertsCorrect(string input)
  {
    var expected = new DateTime(DateTime.UtcNow.Year, 10, 11);

    var value = CliValueConversion.ToDateTime(input);

    value.ShouldBe(expected);
  }

  [Test]
  [TestCase("")]
  [TestCase("a")]
  [TestCase("2012-12-45")]
  [TestCase("2012-45-12")]
  public void InvalidString_ToDateTime_ThrowsException(string input)
  {
    Should.Throw<FormatException>(() => CliValueConversion.ToDateTime(input));
  }

  [Test]
  [TestCase("0:00", 0)]
  [TestCase("0:01", 1)]
  [TestCase("1:01", 61)]
  [TestCase("1:59", 119)]
  [TestCase("23:59", 23 * 60 + 59)]
  public void String_HoursAndMinutesToTimeSpan_ConvertsCorrect(string input, int totalMinutes)
  {
    var expected = TimeSpan.FromMinutes(totalMinutes);

    var value = CliValueConversion.HoursAndMinutesToTimeSpan(input);

    value.ShouldBe(expected);
  }

  [Test]
  [TestCase("")]
  [TestCase("a")]
  [TestCase("2012-12-45")]
  [TestCase(":")]
  [TestCase(":1")]
  [TestCase("1:")]
  [TestCase("1.0")]
  public void InvalidString_HoursAndMinutesToTimeSpan_ThrowsException(string input)
  {
    Should.Throw<FormatException>(() => CliValueConversion.HoursAndMinutesToTimeSpan(input));
  }

  [Test]
  [TestCase("0:00", 0)]
  [TestCase("0:01", 1)]
  [TestCase("1:01", 61)]
  [TestCase("1:59", 119)]
  [TestCase("55:59", 55 * 60 + 59)]
  public void String_MinutesAndSecondsToTimeSpan_ConvertsCorrect(string input, int totalSeconds)
  {
    var expected = TimeSpan.FromSeconds(totalSeconds);

    var value = CliValueConversion.MinutesAndSecondsToTimeSpan(input);

    value.ShouldBe(expected);
  }

  [Test]
  [TestCase("")]
  [TestCase("a")]
  [TestCase("2012-12-45")]
  [TestCase(":")]
  [TestCase(":1")]
  [TestCase("1:")]
  [TestCase("1.0")]
  public void InvalidString_MinutesAndSecondsToTimeSpan_ThrowsException(string input)
  {
    Should.Throw<FormatException>(() => CliValueConversion.MinutesAndSecondsToTimeSpan(input));
  }
}
