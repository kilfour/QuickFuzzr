namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when a configuration expression does not refer to a property.
/// Occurs when attempting to configure a field or method instead of a property
/// in a <c>Configr&lt;T&gt;</c> setup.
/// </summary>
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