using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    public static FuzzrOf<Intent> EndOn<TLeaf>()
    {
        return
            s =>
                {
                    s.TreeLeaves[typeof(T)] = typeof(TLeaf);
                    return new Result<Intent>(Intent.Fixed, s);
                };
    }
}