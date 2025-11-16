using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a Fuzzr that applies configuration based on a generated value from another Fuzzr.
    /// Use for complex configuration scenarios where generation parameters depend on values produced by other Fuzzrs.
    /// </summary>
    public static FuzzrOf<Intent> With<TValue>(
        FuzzrOf<TValue> fuzzr,
        Func<TValue, FuzzrOf<Intent>> configrFactory)
    {
        ArgumentNullException.ThrowIfNull(fuzzr);
        ArgumentNullException.ThrowIfNull(configrFactory);
        return state =>
        {
            state.WithCustomizations[(typeof(T), typeof(TValue))] = (fuzzr.AsObject(), a => configrFactory((TValue)a));
            return new Result<Intent>(Intent.Fixed, state);
        };
    }
}