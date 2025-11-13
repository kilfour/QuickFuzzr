namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when <c>Fuzzr.OneOf&lt;T&gt;</c> is called with one or more negative weights,
/// </summary>
public class NegativeWeightException(string typeName)
    : QuickFuzzrException(BuildMessage(typeName))
{
    private static string BuildMessage(string typeName) =>
$@"Fuzzr.OneOf<{typeName}> cannot have negative weights.

Possible solutions:
- Ensure all weights are non-negative.
- Set the weight to 0 to disable a branch without removing it.
";
}
