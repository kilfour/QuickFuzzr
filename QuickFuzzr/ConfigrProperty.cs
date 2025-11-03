using System.Reflection;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    public static FuzzrOf<Intent> Property<TProperty>(Func<PropertyInfo, bool> predicate, FuzzrOf<TProperty> propertyGenerator)
        => state =>
            {
                state.GeneralCustomizations[predicate] = _ => propertyGenerator.AsObject();
                return new Result<Intent>(Intent.Fixed, state);
            };

    public static FuzzrOf<Intent> Property<TProperty>(Func<PropertyInfo, bool> predicate,
        Func<PropertyInfo, FuzzrOf<TProperty>> func)
        => state =>
            {
                state.GeneralCustomizations[predicate] = a => func(a).AsObject();
                return new Result<Intent>(Intent.Fixed, state);
            };

    public static FuzzrOf<Intent> Property<TProperty>(Func<PropertyInfo, bool> predicate, TProperty value)
        => state =>
            {
                state.GeneralCustomizations[predicate] = _ => Fuzzr.Constant(value).AsObject();
                return new Result<Intent>(Intent.Fixed, state);
            };

    public static FuzzrOf<Intent> Property<TProperty>(Func<PropertyInfo, bool> predicate, Func<PropertyInfo, TProperty> func)
       => state =>
           {
               state.GeneralCustomizations[predicate] = a => Fuzzr.Constant(func(a)).AsObject();
               return new Result<Intent>(Intent.Fixed, state);
           };

}