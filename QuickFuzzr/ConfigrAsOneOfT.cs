using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a generator that configures inheritance resolution for type T to randomly select from the specified derived types.
    /// Use for generating polymorphic object graphs where you need random but controlled type selection from a hierarchy.
    /// </summary>
    public static FuzzrOf<Intent> AsOneOf(params Type[] types)
    {
        return
            s =>
                {
                    s.InheritanceInfo[typeof(T)] = [.. types];
                    return new Result<Intent>(Intent.Fixed, s);
                };
    }
}