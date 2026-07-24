using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    /// <summary>
    /// Creates a Fuzzr that enables automatic generation for public mutable fields.
    /// Field access is disabled by default.
    /// </summary>
    public static FuzzrOf<Intent> EnableFieldAccess() =>
        state =>
        {
            state.FieldAccessEnabled = true;
            return Result.Unit(state);
        };
}
