using QuickFuzzr.UnderTheHood;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong.AsOneOfExceptions;

namespace QuickFuzzr;

public static partial class Configr<T>
{
    /// <summary>
    /// Creates a Fuzzr that configures inheritance resolution for type T to randomly select from the specified derived types.
    /// Randomly choose which subtype to instantiate when calling <see cref="Fuzzr.One{T}()"/>.
    /// </summary>
    public static FuzzrOf<Intent> AsOneOf(params Type[] derivedTypes)
    {
        ValidateTypes(typeof(T), derivedTypes);
        return state =>
        {
            state.InheritanceInfo[typeof(T)] = [.. derivedTypes];
            return new Result<Intent>(Intent.Fixed, state);
        };
    }

    private static void ValidateTypes(Type baseType, Type[] derivedTypes)
    {
        if (derivedTypes.Length == 0) throw new EmptyDerivedTypesException(baseType.Name!);
        if (derivedTypes.Any(t => t == null)) throw new DerivedTypeIsNullException(baseType.Name!);
        var duplicates = GetDuplicates(derivedTypes);
        if (duplicates.Count > 0)
            throw new DuplicateDerivedTypesException(baseType.Name, duplicates);
        var nonAssignableTypes = derivedTypes.Where(t => !baseType.IsAssignableFrom(t)).ToList();
        if (nonAssignableTypes.Count == 0) return;
        throw new DerivedTypeNotAssignableException(
            baseType.Name!,
            DerivedTypeNotAssignableException.Method.AsOneOf,
            nonAssignableTypes);
    }

    private static List<Type> GetDuplicates(Type[] derivedTypes)
        => [.. derivedTypes.GroupBy(t => t).Where(g => g.Count() > 1).Select(g => g.Key)];
}