using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a fuzzr that applies configuration based on a generated value from another fuzzr.
    /// Use for complex configuration scenarios where generation parameters depend on values produced by other fuzzrs.
    /// </summary>
    public static FuzzrOf<Intent> With<TValue>(FuzzrOf<TValue> fuzzr, Func<TValue, FuzzrOf<Intent>> configrFactory)
        => state =>
            {
                state.WithCustomizations[(typeof(T), typeof(TValue))] = (fuzzr.AsObject(), a => configrFactory((TValue)a));
                return new Result<Intent>(Intent.Fixed, state);
            };

}