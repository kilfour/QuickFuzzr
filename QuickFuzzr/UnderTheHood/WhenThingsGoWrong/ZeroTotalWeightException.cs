namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when <c>Fuzzr.OneOf&lt;T&gt;</c> is called with a total weight of zero or less,
/// </summary>
public class ZeroTotalWeightException(string typeName)
    : QuickFuzzrException(BuildMessage(typeName))
{
    private static string BuildMessage(string typeName) =>
$@"Fuzzr.OneOf<{typeName}> cannot have a total weight of zero or less.

Possible solutions:
• Ensure at least one option has a positive weight.
• Use zero weight entries only to disable specific options.
• Adjust weights to ensure the total is greater than zero.
";
}