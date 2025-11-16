namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when QuickFuzzr cannot construct an instance of a type using a factory method because the factory returned null.
/// </summary>
/// <param name="typeName"></param>
public class FactoryConstructionException(string typeName) : QuickFuzzrException(BuildMessage(typeName))
{
    private static string BuildMessage(string typeName) =>
$@"Cannot construct instance of {typeName} using the provided factory method.
The factory returned null.

Possible solutions:
- Return a non-null instance
- Ensure the factory does not depend on uninitialized outer values
";
}
