using System.Linq.Expressions;
using System.Reflection;

namespace NiceCli.Core;

internal class MemberAccessor
{
  private readonly Type _classType;
  private readonly Type _memberType;
  private readonly MemberInfo _memberInfo;
  private readonly PropertyInfo _property;

  private MemberAccessor(Expression expression, Type classType, Type memberType)
  {
    _classType = classType;
    _memberType = memberType;
    _memberInfo = GetMemberInfo(expression);
    _property = _classType.GetProperty(MemberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) ??
      throw new ArgumentException($"{classType.FullName} does not have a property named: {MemberName}.");

    MemberNameKebabCase = _memberInfo.Name.PascalToKebabCase();
  }

  public string MemberName => _memberInfo.Name;
  public string MemberNameKebabCase { get; }

  public static MemberAccessor FromMemberExpression<TClass, TMember>(Expression<Func<TClass, TMember>> expression)
  {
    return new MemberAccessor(expression, typeof(TClass), typeof(TMember));
  }

  public void SetValue(object instance, object value)
  {
    if (instance == null)
      throw new ArgumentNullException(nameof(instance));
    if (!instance.GetType().IsAssignableFrom(_classType))
      throw new ArgumentException($"Instance is of wrong type, expected type: {_classType.FullName}, actual type: {instance.GetType().FullName}.");
    if (value != null && !value.GetType().IsAssignableFrom(_memberType))
      throw new ArgumentException($"Value is of wrong type, expected type: {_memberType.FullName}, actual type: {value.GetType().FullName}.");

    if (value is long && _property.PropertyType == typeof(int))
      value = Convert.ToInt32(value, CultureInfoIso.InvariantCultureWithIso8601);

    _property.SetValue(instance, value);
  }

  private static MemberInfo GetMemberInfo(Expression expression)
  {
    if (expression is MemberExpression memberExpression)
    {
      // Reference type property or field
      return memberExpression.Member;
    }

    if (expression is MethodCallExpression)
    {
      // Reference type method
      throw new NotSupportedException($"{nameof(MemberAccessor)} only supports properties or fields, not method calls.");
    }

    if (expression is UnaryExpression unaryExpression)
    {
      // Property, field of method returning value type
      return GetMemberInfo(unaryExpression);
    }

    if (expression is LambdaExpression lambdaExpression)
      return GetMemberInfo(lambdaExpression.Body);

    throw new ArgumentException("Expression is not a supported member accessor expression.");
  }

  private static MemberInfo GetMemberInfo(UnaryExpression unaryExpression)
  {
    if (unaryExpression.Operand is MethodCallExpression methodExpression)
    {
      return methodExpression.Method;
    }

    return ((MemberExpression) unaryExpression.Operand).Member;
  }
}
