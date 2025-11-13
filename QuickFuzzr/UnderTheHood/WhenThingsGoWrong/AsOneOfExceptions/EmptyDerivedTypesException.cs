namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong.AsOneOfExceptions;

/// <summary>
/// Thrown when <see cref="Configr{T}.AsOneOf"/> configuration is provided with an empty array of derived types.
/// </summary>
public class EmptyDerivedTypesException(string baseType)
    : QuickFuzzrException(BuildMessage(baseType))
{
    private static string BuildMessage(string baseType) =>
$@"No derived types were provided to AsOneOf for base type {baseType}.

Possible solutions:
- Provide at least one derived type in Configr<{baseType}>.AsOneOf(...).
- Ensure that the derived types array is not empty.
";
}