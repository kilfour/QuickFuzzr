using QuickFuzzr.UnderTheHood;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong.AsOneOfExceptions;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a Fuzzr that configures type T to terminate recursion by generating TEnd instances instead of further recursion.
    /// Use for controlling complex object graphs by specifying End types that should stop further depth expansion in recursive structures.
    /// </summary>
    public static FuzzrOf<Intent> EndOn<TEnd>()
    {
        var baseType = typeof(T);
        var endingType = typeof(TEnd);
        if (!baseType.IsAssignableFrom(endingType))
            throw new DerivedTypeNotAssignableException(
                baseType.Name!,
                DerivedTypeNotAssignableException.Method.EndOn,
                [endingType]);
        return
            s =>
                {
                    s.Endings[baseType] = endingType;
                    return new Result<Intent>(Intent.Fixed, s);
                };
    }
}