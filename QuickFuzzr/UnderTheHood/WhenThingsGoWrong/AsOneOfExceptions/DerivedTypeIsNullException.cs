namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong.AsOneOfExceptions;

public class DerivedTypeIsNullException(string baseType)
    : QuickFuzzrException(BuildMessage(baseType))
{
    private static string BuildMessage(string baseType) =>
$@"A null derived type was provided to AsOneOf for base type {baseType}.

Possible solutions:
- Ensure that all derived types in Configr<{baseType}>.AsOneOf(...) are non-null.
";
}