using System.Linq.Expressions;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    public static FuzzrOf<Intent> Property<TProperty>(Expression<Func<T, TProperty>> func, FuzzrOf<TProperty> propertyGenerator)
        => state =>
            {
                state.Customizations[func.AsPropertyInfo()] = propertyGenerator.AsObject();
                return new Result<Intent>(Intent.Fixed, state);
            };

    public static FuzzrOf<Intent> Property<TProperty>(Expression<Func<T, TProperty>> func, TProperty value)
        => state =>
            {
                state.Customizations[func.AsPropertyInfo()] = Fuzzr.Constant(value).AsObject();
                return new Result<Intent>(Intent.Fixed, state);
            };

}