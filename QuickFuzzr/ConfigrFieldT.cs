using System.Linq.Expressions;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a Fuzzr that configures a specific field to use a custom Fuzzr for its values.
    /// </summary>
    public static FuzzrOf<Intent> Field<TField>(
        Expression<Func<T, TField>> expression,
        FuzzrOf<TField> fuzzr)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(fuzzr);
        return FieldInternal(expression, fuzzr);
    }

    /// <summary>
    /// Creates a Fuzzr that configures a specific field to use a constant value.
    /// </summary>
    public static FuzzrOf<Intent> Field<TField>(
        Expression<Func<T, TField>> expression,
        TField value)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return FieldInternal(expression, Fuzzr.Constant(value));
    }

    private static FuzzrOf<Intent> FieldInternal<TField>(
        Expression<Func<T, TField>> expression,
        FuzzrOf<TField> fuzzr)
    {
        var fieldName = expression.AsFieldInfo().Name;
        var targetType = typeof(T);
        return state =>
        {
            state.FieldCustomizations[(targetType, fieldName)] = fuzzr.AsObject();
            return Result.Unit(state);
        };
    }
}
