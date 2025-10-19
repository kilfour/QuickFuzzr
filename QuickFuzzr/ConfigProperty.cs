using System.Linq.Expressions;
using System.Reflection;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static class Config
{
    public static Generator<Unit> Property<TProperty>(Func<PropertyInfo, bool> predicate, Generator<TProperty> propertyGenerator)
        => state =>
            {
                state.GeneralCustomizations[predicate] = propertyGenerator.AsObject();
                return new Result<Unit>(Unit.Instance, state);
            };

}