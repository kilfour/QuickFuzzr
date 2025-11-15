using System.Reflection;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    /// <summary>
    /// Creates a Fuzzr that configures custom property generation for properties matching the predicate using the specified Fuzzr.
    /// Use for applying specialized generation rules to groups of properties based on their characteristics or metadata.
    /// </summary>
    public static FuzzrOf<Intent> Property<TProperty>(
        Func<PropertyInfo, bool> predicate,
        FuzzrOf<TProperty> fuzzr)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(fuzzr);
        return state =>
                {
                    state.GeneralCustomizations[predicate] = _ => fuzzr.AsObject();
                    return new Result<Intent>(Intent.Fixed, state);
                };
    }

    /// <summary>
    /// Creates a Fuzzr that configures custom property generation using a factory function that receives property metadata.
    /// Use when property generation logic depends on property characteristics like name, type, or custom attributes.
    /// </summary>
    public static FuzzrOf<Intent> Property<TProperty>(Func<PropertyInfo, bool> predicate,
        Func<PropertyInfo, FuzzrOf<TProperty>> factoryFunc)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(factoryFunc);
        return state =>
                {
                    state.GeneralCustomizations[predicate] = a => factoryFunc(a).AsObject();
                    return new Result<Intent>(Intent.Fixed, state);
                };
    }

    /// <summary>
    /// Creates a Fuzzr that configures properties matching the predicate to always generate the same constant value.
    /// Use for setting fixed values on properties that should never vary, such as constants, default values, or test fixtures.
    /// </summary>
    public static FuzzrOf<Intent> Property<TProperty>(
        Func<PropertyInfo, bool> predicate,
        TProperty value)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return state =>
                {
                    state.GeneralCustomizations[predicate] = _ => Fuzzr.Constant(value).AsObject();
                    return new Result<Intent>(Intent.Fixed, state);
                };
    }

    /// <summary>
    /// Creates a Fuzzr that configures properties matching the predicate to generate values based on property metadata.
    /// Use when property values should be derived from property characteristics like name, declaring type, or custom attributes.
    /// </summary>
    public static FuzzrOf<Intent> Property<TProperty>(
        Func<PropertyInfo, bool> predicate,
        Func<PropertyInfo, TProperty> factoryFunction)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(factoryFunction);
        return state =>
        {
            state.GeneralCustomizations[predicate] = a => Fuzzr.Constant(factoryFunction(a)).AsObject();
            return new Result<Intent>(Intent.Fixed, state);
        };
    }
}