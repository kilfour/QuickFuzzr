using QuickFuzzr;

namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong.AsOneOfExceptions;

/// <summary>
/// Thrown when <see cref="Configr{T}.AsOneOf"/> configuration specifies duplicate derived types.
/// </summary>
public class DuplicateDerivedTypesException(string baseType, List<Type> duplicateTypes)
    : QuickFuzzrException(BuildMessage(baseType, duplicateTypes))
{
    private static string BuildMessage(string baseType, List<Type> duplicateTypes)
        => duplicateTypes.Count == 1
            ? BuildMessageForSingleType(baseType, duplicateTypes[0].Name)
            : BuildMessageForMultipleTypes(baseType, duplicateTypes);

    private static string BuildMessageForSingleType(string baseType, string derivedType) =>
$@"A duplicate derived type was provided to AsOneOf for base type {baseType}: {derivedType}.

Possible solutions:
- Ensure {derivedType} only appears once in Configr<{baseType}>.AsOneOf(...).
";

    private static string BuildMessageForMultipleTypes(string baseType, List<Type> duplicateTypes)
    {
        var duplicateList = string.Join(Environment.NewLine, duplicateTypes.Select(t => $"- {t.Name}"));
        return
$@"Duplicate derived types were provided to AsOneOf for base type {baseType}:
{duplicateList}

Possible solutions:
- Ensure each derived type in Configr<{baseType}>.AsOneOf(...) is unique.
";
    }
}