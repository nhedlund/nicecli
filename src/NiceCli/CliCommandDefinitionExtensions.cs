using System.Globalization;
using System.Linq.Expressions;
using NiceCli.Core;

namespace NiceCli;

public static class CliCommandExtensions
{
  public static CliCommandDefinition<TCommand> Example<TCommand>(this CliCommandDefinition<TCommand> commandDefinition, string example) where TCommand : ICliCommand
  {
    if (string.IsNullOrWhiteSpace(example))
      throw new ArgumentException($"{nameof(example)} is null or empty.");

    commandDefinition.Examples.Add(example);
    return commandDefinition;
  }

  public static CliCommandDefinition<TCommand> LearnMore<TCommand>(this CliCommandDefinition<TCommand> commandDefinition, string learnMore) where TCommand : ICliCommand
  {
    if (string.IsNullOrWhiteSpace(learnMore))
      throw new ArgumentException($"{nameof(learnMore)} is null or empty.");

    commandDefinition.LearnMores.Add(learnMore);
    return commandDefinition;
  }

  public static CliCommandDefinition<TCommand> PositionalParameter<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, string>> property,
    string description,
    CliOptionality optionality = CliOptionality.Mandatory) where TCommand : ICliCommand
  {
    return command.PositionalParameter(property, optionality, description, value => value);
  }

  public static CliCommandDefinition<TCommand> PositionalParameter<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, long>> property,
    string description,
    CliOptionality optionality = CliOptionality.Mandatory) where TCommand : ICliCommand
  {
    return command.PositionalParameter(property, optionality, description, CliValueConversion.ToLong);
  }

  public static CliCommandDefinition<TCommand> PositionalParameter<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, decimal>> property,
    string description,
    CliOptionality optionality = CliOptionality.Mandatory) where TCommand : ICliCommand
  {
    return command.PositionalParameter(property, optionality, description, CliValueConversion.ToDecimal);
  }

  public static CliCommandDefinition<TCommand> PositionalParameter<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, DateTime>> property,
    string description,
    CliOptionality optionality = CliOptionality.Mandatory) where TCommand : ICliCommand
  {
    return command.PositionalParameter(property, optionality, description, CliValueConversion.ToDateTime);
  }

  public static CliCommandDefinition<TCommand> Flag<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, bool>> property,
    string description,
    char shortName = ' ') where TCommand : ICliCommand
  {
    return command.Flag(property, CliVisibility.Visible, description, shortName);
  }

  public static CliCommandDefinition<TCommand> HiddenFlag<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, bool>> property,
    string description,
    char shortName = ' ') where TCommand : ICliCommand
  {
    return command.Flag(property, CliVisibility.Hidden, description, shortName);
  }

  public static CliCommandDefinition<TCommand> Option<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, string>> property,
    string description,
    string parameter,
    char shortName = ' ') where TCommand : ICliCommand
  {
    return command.Option(property, CliVisibility.Visible, description, parameter, value => value, shortName);
  }

  public static CliCommandDefinition<TCommand> HiddenOption<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, string>> property,
    string description,
    string parameter,
    char shortName = ' ') where TCommand : ICliCommand
  {
    return command.Option(property, CliVisibility.Hidden, description, parameter, value => value, shortName);
  }

  public static CliCommandDefinition<TCommand> Option<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, long>> property,
    string description,
    string parameter,
    char shortName = ' ') where TCommand : ICliCommand
  {
    return command.Option(property, CliVisibility.Visible, description, parameter, value => long.Parse(value, CultureInfo.InvariantCulture), shortName);
  }

  public static CliCommandDefinition<TCommand> HiddenOption<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, long>> property,
    string description,
    string parameter,
    char shortName = ' ') where TCommand : ICliCommand
  {
    return command.Option(property, CliVisibility.Hidden, description, parameter, value => long.Parse(value, CultureInfo.InvariantCulture), shortName);
  }

  public static CliCommandDefinition<TCommand> Option<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, decimal>> property,
    string description,
    string parameter,
    char shortName = ' ') where TCommand : ICliCommand
  {
    return command.Option(property, CliVisibility.Visible, description, parameter, value => decimal.Parse(value, CultureInfo.InvariantCulture), shortName);
  }

  public static CliCommandDefinition<TCommand> HiddenOption<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, decimal>> property,
    string description,
    string parameter,
    char shortName = ' ') where TCommand : ICliCommand
  {
    return command.Option(property, CliVisibility.Hidden, description, parameter, value => decimal.Parse(value, CultureInfo.InvariantCulture), shortName);
  }

  public static CliCommandDefinition<TCommand> Option<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, DateTime>> property,
    string description,
    string parameter,
    char shortName = ' ') where TCommand : ICliCommand
  {
    return command.Option(property, CliVisibility.Visible, description, parameter, DateTime.Parse, shortName);
  }

  public static CliCommandDefinition<TCommand> HiddenOption<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, DateTime>> property,
    string description,
    string parameter,
    char shortName = ' ') where TCommand : ICliCommand
  {
    return command.Option(property, CliVisibility.Hidden, description, parameter, DateTime.Parse, shortName);
  }

  private static CliCommandDefinition<TCommand> Flag<TCommand>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, bool>> property,
    CliVisibility visibility,
    string description,
    char shortName) where TCommand : ICliCommand
  {
    var propertyAccessor = MemberAccessor.FromMemberExpression(property);

    var flag = new CliFlag(
      propertyAccessor.MemberName,
      shortName,
      propertyAccessor.MemberNameKebabCase,
      description,
      visibility,
      instance => propertyAccessor.SetValue(instance, true));

    command.Parameters.Add(flag);
    return command;
  }

  private static CliCommandDefinition<TCommand> Option<TCommand, TValue>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, TValue>> property,
    CliVisibility visibility,
    string description,
    string parameter,
    Func<string, TValue> parseValue,
    char shortName) where TCommand : ICliCommand
  {
    var propertyAccessor = MemberAccessor.FromMemberExpression(property);

    var option = new CliOption(
      propertyAccessor.MemberName,
      shortName,
      propertyAccessor.MemberNameKebabCase,
      parameter,
      description,
      visibility,
      (instance, value) => propertyAccessor.SetValue(instance, parseValue(value)!));

    command.Parameters.Add(option);
    return command;
  }

  private static CliCommandDefinition<TCommand> PositionalParameter<TCommand, TValue>(
    this CliCommandDefinition<TCommand> command,
    Expression<Func<TCommand, TValue>> property,
    CliOptionality optionality,
    string description,
    Func<string, TValue> parseValue) where TCommand : ICliCommand
  {
    var propertyAccessor = MemberAccessor.FromMemberExpression(property);

    var option = new CliPositionalParameter(
      propertyAccessor.MemberName,
      description,
      optionality,
      (instance, value) => propertyAccessor.SetValue(instance, parseValue(value)!));

    command.Parameters.Add(option);
    return command;
  }
}
