using QuickFuzzr.Instruments;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    public static Generator<Unit> Depth(int min, int max)
        => state => Chain.It(() => state.DepthConstraints[typeof(T)] = new(min, max), Result.Unit(state));



    public static Generator<Unit> GenerateAsOneOf(params Type[] types)
    {
        return
            s =>
                {
                    s.InheritanceInfo[typeof(T)] = types.ToList();
                    return new Result<Unit>(Unit.Instance, s);
                };
    }

    public static Generator<Unit> TreeLeaf<TLeaf>()
    {
        return
            s =>
                {
                    s.TreeLeaves[typeof(T)] = typeof(TLeaf);
                    return new Result<Unit>(Unit.Instance, s);
                };
    }
}