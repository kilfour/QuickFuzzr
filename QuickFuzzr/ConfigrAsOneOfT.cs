using QuickFuzzr.UnderTheHood;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a generator that configures inheritance resolution for type T to randomly select from the specified derived types.
    /// Use for generating polymorphic object graphs where you need random but controlled type selection from a hierarchy.
    /// </summary>
    public static FuzzrOf<Intent> AsOneOf(params Type[] derivedTypes)
    {
        return
            s =>
                {
                    EnsureAllTypesAreAssignableToBaseType(typeof(T), derivedTypes);
                    s.InheritanceInfo[typeof(T)] = [.. derivedTypes];
                    return new Result<Intent>(Intent.Fixed, s);
                };
    }

    private static void EnsureAllTypesAreAssignableToBaseType(Type baseType, Type[] derivedTypes)
    {
        var nonAssignableTypes = derivedTypes.Where(t => !baseType.IsAssignableFrom(t)).ToList();
        if (nonAssignableTypes.Count == 0) return;
        throw new DerivedTypeNotAssignableException(baseType.Name!, nonAssignableTypes);
    }
}