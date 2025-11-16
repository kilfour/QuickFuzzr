namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;


/// <summary>
/// Thrown when QuickFuzzr cannot find a constructor on type T matching the configured parameter list.
/// </summary>
public class ConstructorNotFoundException(string typeName, Type[] argumentTypes)
    : QuickFuzzrException(BuildMessage(typeName, argumentTypes))
{
    private static string BuildMessage(string typeName, Type[] argumentTypes)
    {
        var argumentList = string.Join(", ", argumentTypes.Select(t => t.Name));
        var typeString = argumentTypes.Length == 1 ? "type" : "types";
        return
$@"Cannot construct instance of {typeName}.
No matching constructor found for argument {typeString}: ({argumentList}).

Possible solutions:
- Add a constructor with the required signature.
- Verify the argument types order is correct.
- Update Configr<Person>.Construct(...) to match an existing constructor.
";
    }
}