using System.Linq.Expressions;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Config<T>
{
    public static Generator<Unit> Property<TProperty>(Expression<Func<T, TProperty>> func, Generator<TProperty> propertyGenerator)
        => state =>
            {
                state.Customizations[func.AsPropertyInfo()] = propertyGenerator.AsObject();
                return new Result<Unit>(Unit.Instance, state);
            };

    public static Generator<Unit> Property<TProperty>(Expression<Func<T, TProperty>> func, TProperty value)
        => state =>
            {
                state.Customizations[func.AsPropertyInfo()] = Fuzzr.Constant(value).AsObject();
                return new Result<Unit>(Unit.Instance, state);
            };

}