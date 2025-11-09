namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

public class PropertyConfigurationException(string typeName, string expression)
    : QuickFuzzrException(BuildMessage(typeName, expression))
{
    private static string BuildMessage(string typeName, string expression) =>
$@"Cannot configure expression '{expression}'.

It does not refer to a property.
Fields and methods are not supported by default.

Possible solutions:
• Use a property selector (e.g. a => a.PropertyName).
• Then pass it to Configr<{typeName}>.Property(...) to configure generation.
";
}