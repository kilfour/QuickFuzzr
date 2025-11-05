using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a generator that configures type T to terminate recursion by generating TLeaf instances instead of further recursion.
    /// Use for controlling complex object graphs by specifying leaf types that should stop further depth expansion in recursive structures.
    /// </summary>
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