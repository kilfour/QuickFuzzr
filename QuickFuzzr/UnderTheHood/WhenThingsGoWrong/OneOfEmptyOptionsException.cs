namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

public sealed class OneOfEmptyOptionsException(string typeName)
    : QuickFuzzrException(BuildMessage(typeName))
{
    private static string BuildMessage(string typeName) =>
$@"Fuzzr.OneOf<{typeName}> cannot select from an empty sequence.

Possible solutions:
• Provide at least one option (ensure the sequence is non-empty).
• Use a fallback: Fuzzr.OneOf(values).WithDefault(default!)
• Guard upstream: values.Any() ? Fuzzr.OneOf(values) : Fuzzr.Constant(default!).
";
}