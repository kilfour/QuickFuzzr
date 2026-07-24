namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when a field configuration expression does not refer to a field.
/// </summary>
public class FieldConfigurationException(string typeName, string expression)
    : QuickFuzzrException(BuildMessage(typeName, expression))
{
    private static string BuildMessage(string typeName, string expression) =>
$@"Cannot configure expression '{expression}'.

It does not refer to a field.

Possible solutions:
- Use a field selector (e.g. a => a.FieldName).
- Then pass it to Configr<{typeName}>.Field(...) to configure generation.
";
}
