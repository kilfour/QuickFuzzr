using System.Reflection;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    /// <summary>
    /// Configures fields matching a predicate to use a custom Fuzzr.
    /// </summary>
    public static FuzzrOf<Intent> Field<TField>(
        Func<FieldInfo, bool> predicate,
        FuzzrOf<TField> fuzzr)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(fuzzr);
        return FieldInternal(predicate, _ => fuzzr);
    }

    /// <summary>
    /// Configures fields matching a predicate using a field-aware Fuzzr factory.
    /// </summary>
    public static FuzzrOf<Intent> Field<TField>(
        Func<FieldInfo, bool> predicate,
        Func<FieldInfo, FuzzrOf<TField>> factory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(factory);
        return FieldInternal(predicate, factory);
    }

    /// <summary>
    /// Configures fields matching a predicate to use a constant value.
    /// </summary>
    public static FuzzrOf<Intent> Field<TField>(
        Func<FieldInfo, bool> predicate,
        TField value)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return FieldInternal(predicate, _ => Fuzzr.Constant(value));
    }

    /// <summary>
    /// Configures fields matching a predicate using a field-aware value factory.
    /// </summary>
    public static FuzzrOf<Intent> Field<TField>(
        Func<FieldInfo, bool> predicate,
        Func<FieldInfo, TField> factory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(factory);
        return FieldInternal(predicate, field => Fuzzr.Constant(factory(field)));
    }

    private static FuzzrOf<Intent> FieldInternal<TField>(
        Func<FieldInfo, bool> predicate,
        Func<FieldInfo, FuzzrOf<TField>> factory) =>
        state =>
        {
            if (!state.GeneralFieldCustomizations.ContainsKey(predicate))
                state.GeneralFieldCustomizationOrder.Add(predicate);
            state.GeneralFieldCustomizations[predicate] = field => factory(field).AsObject();
            return Result.Unit(state);
        };
}
