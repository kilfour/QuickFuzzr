using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    public static Generator<Unit> EndOn<TLeaf>()
    {
        return
            s =>
                {
                    s.TreeLeaves[typeof(T)] = typeof(TLeaf);
                    return new Result<Unit>(Unit.Instance, s);
                };
    }
}