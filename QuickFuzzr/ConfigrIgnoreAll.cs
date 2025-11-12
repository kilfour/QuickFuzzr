using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    /// <summary>
    /// Creates a fuzzr that configures all properties to be ignored during automatic generation.
    /// Use when you want complete manual control over property population and no automatic property filling should occur.
    /// </summary>
    public static FuzzrOf<Intent> IgnoreAll()
        => state => Chain.It(() => state.GeneralStuffToIgnore.Add(_ => true), Result.Unit(state));
}