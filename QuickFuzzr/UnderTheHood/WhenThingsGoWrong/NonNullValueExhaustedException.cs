namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when a fuzzr failed to produce a non-null value within the allowed retry limit.
/// </summary>
public sealed class NonNullValueExhaustedException(string typeName, int attempts)
    : QuickFuzzrException(BuildMessage(typeName, attempts))
{
    private static string BuildMessage(string typeName, int attempts) =>
$@"Could not produce a non-null value of type {typeName} after {attempts} {(attempts == 1 ? "attempt" : "attempts")}.

Possible solutions:
• Reduce the null probability: Configr.Primitive<{typeName}?>(Fuzzr.Nullable<{typeName}>(0.05))
• Provide a fallback: .WithDefault(default({typeName}))
• Widen the value space (adjust fuzzrs/filters)
• Increase the retry limit globally: Configr.RetryLimit(256)";
}