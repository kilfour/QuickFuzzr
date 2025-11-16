using System.Linq.Expressions;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a Fuzzr that configures a specific property to use a custom Fuzzr for its values.
    /// Use for overriding default generation behavior for individual properties with specialized generation logic.
    /// </summary>
    public static FuzzrOf<Intent> Property<TProperty>(
        Expression<Func<T, TProperty>> expression,
        FuzzrOf<TProperty> fuzzr)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(fuzzr);
        var propertyName = expression.AsPropertyInfo().Name;
        var targetType = typeof(T);
        return state =>
        {
            state.Customizations[(targetType, propertyName)] = fuzzr.AsObject();
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    /// <summary>
    /// Creates a Fuzzr that configures a specific property to always generate the same constant value.
    /// Use for setting fixed values on specific properties that should never vary across generated instances.
    /// </summary>
    public static FuzzrOf<Intent> Property<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
    {
        ArgumentNullException.ThrowIfNull(expression);
        var propertyName = expression.AsPropertyInfo().Name;
        var targetType = typeof(T);
        return state =>
        {
            state.Customizations[(targetType, propertyName)] = Fuzzr.Constant(value).AsObject();
            return new Result<Intent>(Intent.Fixed, state);
        };
    }
}