using System.Linq.Expressions;
using System.Reflection;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

namespace QuickFuzzr.UnderTheHood;

public static class ExpressionExtensions
{
	public static PropertyInfo AsPropertyInfo<TTarget, TProperty>(this Expression<Func<TTarget, TProperty>> expression)
	{
		if (expression.Body is MemberExpression memberExpr)
		{
			if (memberExpr.Member is PropertyInfo property)
				return property;
		}

		if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
		{
			if (unaryMember.Member is PropertyInfo property)
				return property;
		}

		throw new PropertyConfigurationException(typeof(TTarget).Name, expression.ToString());
	}

	public static FieldInfo AsFieldInfo<TTarget, TField>(this Expression<Func<TTarget, TField>> expression)
	{
		if (expression.Body is MemberExpression memberExpr && memberExpr.Member is FieldInfo field)
			return field;

		if (expression.Body is UnaryExpression unary &&
			unary.Operand is MemberExpression unaryMember &&
			unaryMember.Member is FieldInfo convertedField)
			return convertedField;

		throw new FieldConfigurationException(typeof(TTarget).Name, expression.ToString());
	}
}
