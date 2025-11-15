using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    /// <summary>
    /// Sets the global retry limit used by Fuzzrs that may need to attempt multiple
    /// value productions before succeeding, such as those created with <c>.Unique(...)</c>.
    /// Use to control how many times Fuzzrs retry before giving up.
    /// </summary>
    public static FuzzrOf<Intent> RetryLimit(int limit) =>
        state =>
        {
            state.SetRetryLimit(limit);
            return Result.Unit(state);
        };
}