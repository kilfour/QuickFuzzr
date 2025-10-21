using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    public static FuzzrOf<Intent> Depth(int min, int max)
        => state => Chain.It(() => state.DepthConstraints[typeof(T)] = new(min, max), Result.Unit(state));
}