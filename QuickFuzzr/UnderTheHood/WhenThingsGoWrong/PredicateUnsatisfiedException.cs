namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when a filtered fuzzr cannot satisfy its predicate within the allowed retry limit.
/// </summary>
public sealed class PredicateUnsatisfiedException(string typeName, int attempts)
    : QuickFuzzrException(BuildMessage(typeName, attempts))
{
    private static string BuildMessage(string typeName, int attempts) =>
$@"Could not find a value of type {typeName} that satisfies the predicate after {attempts} {(attempts == 1 ? "attempt" : "attempts")}.

Possible solutions:
- Relax or fix the predicate
- Widen the fuzzr's value space
- Provide a fallback: .WithDefault(...)
- Increase the retry limit globally: Configr.RetryLimit(256)";
}