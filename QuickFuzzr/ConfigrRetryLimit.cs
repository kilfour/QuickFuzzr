using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    /// <summary>
    /// Sets the global retry limit used by fuzzrs that may need to attempt multiple
    /// value productions before succeeding, such as those created with <c>.Unique(...)</c>.
    /// Use to control how many times fuzzrs retry before giving up.
    /// </summary>
    public static FuzzrOf<Intent> RetryLimit(int limit)
        => state => Chain.It(() => state.SetRetryLimit(limit), Result.Unit(state));
}