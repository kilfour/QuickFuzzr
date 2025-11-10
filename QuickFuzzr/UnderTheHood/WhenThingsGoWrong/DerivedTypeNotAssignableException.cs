namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when <see cref="Configr{T}.AsOneOf"/> configuration specifies a derived type that is not assignable to the base type.
/// </summary>
public class DerivedTypeNotAssignableException(string baseType, List<Type> nonAssignableTypes)
    : QuickFuzzrException(BuildMessage(baseType, nonAssignableTypes))
{
    private static string BuildMessage(string baseType, List<Type> nonAssignableTypes)
        => nonAssignableTypes.Count == 1
            ? BuildMessageForSingleType(baseType, nonAssignableTypes[0].Name)
            : BuildMessageForMultipleTypes(baseType, nonAssignableTypes);

    private static string BuildMessageForMultipleTypes(string baseType, List<Type> nonAssignableTypes)
    {
        return
$@"The following types are not assignable to the base type {baseType}:
{string.Join(Environment.NewLine, nonAssignableTypes.Select(t => $"• {t.Name}"))}

Possible solutions:
• Use compatible types in Configr<{baseType}>.AsOneOf(...).
• Ensure all listed types inherit from or implement Person.
";
    }

    private static string BuildMessageForSingleType(string baseType, string derivedType) =>
$@"The type {derivedType} is not assignable to the base type {baseType}.

Possible solutions:
• Use compatible types in Configr<{baseType}>.AsOneOf(...).
• Ensure {derivedType} inherits from or implements {baseType}.
";
}
