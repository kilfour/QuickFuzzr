using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr
{
    public static FuzzrOf<Intent> IgnoreAll()
        => state => Chain.It(() => state.GeneralStuffToIgnore.Add(_ => true), Result.Unit(state));
}