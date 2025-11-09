namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when <c>Fuzzr.OneOf&lt;T&gt;</c> is called with an empty sequence,
/// preventing a value from being selected.
/// </summary>
public class OneOfEmptyOptionsException(string typeName)
    : QuickFuzzrException(BuildMessage(typeName))
{
    private static string BuildMessage(string typeName) =>
$@"Fuzzr.OneOf<{typeName}> cannot select from an empty sequence.

Possible solutions:
• Provide at least one option (ensure the sequence is non-empty).
• Use a fallback: Fuzzr.OneOf(values).WithDefault()
• Guard upstream: values.Any() ? Fuzzr.OneOf(values) : Fuzzr.Constant(default!).
";
}
