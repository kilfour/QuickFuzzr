using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a generator that configures depth constraints for type T to control recursive object graph generation.
    /// Use for preventing infinite recursion in complex nested structures and ensuring generation terminates at reasonable depths.
    /// </summary>
    public static FuzzrOf<Intent> Depth(int min, int max)
        => state => Chain.It(() => state.DepthConstraints[typeof(T)] = new(min, max), Result.Unit(state));
}