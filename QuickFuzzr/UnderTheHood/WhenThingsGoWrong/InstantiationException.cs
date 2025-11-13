namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when QuickFuzzr attempts to instantiate an abstract type.
/// Wraps the original <see cref="MemberAccessException"/>.
/// </summary>
public class InstantiationException(string typeName, MemberAccessException innerException)
    : QuickFuzzrException(BuildMessage(typeName), innerException)
{
    private static string BuildMessage(string typeName) =>
$@"Cannot generate an instance of the abstract class {typeName}.

Possible solution:
- Register one or more concrete subtype(s): Configr<{typeName}>.AsOneOf(...)
";
}
