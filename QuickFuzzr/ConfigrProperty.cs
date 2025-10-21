using System.Linq.Expressions;
using System.Reflection;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    public static FuzzrOf<Intent> Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> propertyGenerator)
        => state =>
            {
                state.GeneralCustomizations[predicate] = propertyGenerator.AsObject();
                return new Result<Intent>(Intent.Fixed, state);
            };

}