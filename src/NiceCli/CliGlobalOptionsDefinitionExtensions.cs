using System.Linq.Expressions;
using NiceCli.Core;

namespace NiceCli;

public static class CliGlobalOptionsDefinitionExtensions
{
  private const string DefaultValue = "value";

  public static CliGlobalOptions<TGlobalOptions> Flag<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, bool>> property,
    string description,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.Flag(property, CliVisibility.Visible, description, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenFlag<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, bool>> property,
    string description,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.Flag(property, CliVisibility.Hidden, description, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, string>> property,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    return app.Option(property, description, DefaultValue, value => value, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, string>> property,
    string description,
    string parameter = DefaultValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.Option(property, description, parameter, value => value, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, string>> property,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    return app.HiddenOption(property, description, DefaultValue, value => value, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, string>> property,
    string description,
    string parameter = DefaultValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.HiddenOption(property, description, parameter, value => value, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, long>> property,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    return app.Option(property, description, DefaultValue, CliValueConversion.ToLong, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, long>> property,
    string description,
    string parameter = DefaultValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.Option(property, description, parameter, CliValueConversion.ToLong, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, long>> property,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    return app.HiddenOption(property, description, DefaultValue, CliValueConversion.ToLong, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, long>> property,
    string description,
    string parameter = DefaultValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.HiddenOption(property, description, parameter, CliValueConversion.ToLong, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, double>> property,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    return app.Option(property, description, DefaultValue, CliValueConversion.ToDouble, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, double>> property,
    string description,
    string parameter = DefaultValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.Option(property, description, parameter, CliValueConversion.ToDouble, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, double>> property,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    return app.HiddenOption(property, description, DefaultValue, CliValueConversion.ToDouble, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, double>> property,
    string description,
    string parameter = DefaultValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.HiddenOption(property, description, parameter, CliValueConversion.ToDouble, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, decimal>> property,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    return app.Option(property, description, DefaultValue, CliValueConversion.ToDecimal, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, decimal>> property,
    string description,
    string parameter = DefaultValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.Option(property, description, parameter, CliValueConversion.ToDecimal, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, decimal>> property,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    return app.HiddenOption(property, description, DefaultValue, CliValueConversion.ToDecimal, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, decimal>> property,
    string description,
    string parameter = DefaultValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.HiddenOption(property, description, parameter, CliValueConversion.ToDecimal, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, DateTime>> property,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    return app.Option(property, description, DefaultValue, CliValueConversion.ToDateTime, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, DateTime>> property,
    string description,
    string parameter = DefaultValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.Option(property, description, parameter, CliValueConversion.ToDateTime, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, DateTime>> property,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    return app.HiddenOption(property, description, DefaultValue, CliValueConversion.ToDateTime, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, DateTime>> property,
    string description,
    string parameter = DefaultValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.HiddenOption(property, description, parameter, CliValueConversion.ToDateTime, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> Configure<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Action<TGlobalOptions> configure) where TGlobalOptions : class, new()
  {
    app.OnConfigured = o => configure((TGlobalOptions) o);
    return app;
  }

  private static CliGlobalOptions<TGlobalOptions> Flag<TGlobalOptions>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, bool>> property,
    CliVisibility visibility,
    string description,
    char shortName) where TGlobalOptions : class, new()
  {
    var propertyAccessor = MemberAccessor.FromMemberExpression(property);

    var flag = new CliFlag(
      propertyAccessor.MemberName,
      shortName,
      propertyAccessor.MemberNameKebabCase,
      description,
      visibility,
      globalOptions => propertyAccessor.SetValue(globalOptions, true));

    app.Parameters.Add(flag);
    return app;
  }

  public static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions, TValue>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, TValue>> property,
    string description,
    string parameter,
    Func<string, TValue> parseValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.Option(property, CliVisibility.Visible, description, parameter, parseValue, shortName);
  }

  public static CliGlobalOptions<TGlobalOptions> HiddenOption<TGlobalOptions, TValue>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, TValue>> property,
    string description,
    string parameter,
    Func<string, TValue> parseValue,
    char shortName = ' ') where TGlobalOptions : class, new()
  {
    return app.Option(property, CliVisibility.Hidden, description, parameter, parseValue, shortName);
  }

  private static CliGlobalOptions<TGlobalOptions> Option<TGlobalOptions, TValue>(
    this CliGlobalOptions<TGlobalOptions> app,
    Expression<Func<TGlobalOptions, TValue>> property,
    CliVisibility visibility,
    string description,
    string parameter,
    Func<string, TValue> parseValue,
    char shortName) where TGlobalOptions : class, new()
  {
    var propertyAccessor = MemberAccessor.FromMemberExpression(property);

    var option = new CliOption(
      propertyAccessor.MemberName,
      shortName,
      propertyAccessor.MemberNameKebabCase,
      parameter,
      description,
      visibility,
      (globalOptions, value) => propertyAccessor.SetValue(globalOptions, parseValue(value)!));

    app.Parameters.Add(option);
    return app;
  }
}
