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
        Expression<Func<T, TProperty>> predicate,
        FuzzrOf<TProperty> fuzzr)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(fuzzr);
        return state =>
        {
            state.Customizations[(predicate.AsPropertyInfo(), typeof(T))] = fuzzr.AsObject();
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    /// <summary>
    /// Creates a Fuzzr that configures a specific property to always generate the same constant value.
    /// Use for setting fixed values on specific properties that should never vary across generated instances.
    /// </summary>
    public static FuzzrOf<Intent> Property<TProperty>(Expression<Func<T, TProperty>> predicate, TProperty value)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return
            state =>
            {
                state.Customizations[(predicate.AsPropertyInfo(), typeof(T))] = Fuzzr.Constant(value).AsObject();
                return new Result<Intent>(Intent.Fixed, state);
            };
    }
}