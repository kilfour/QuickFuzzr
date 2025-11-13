namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when QuickFuzzr cannot produce a new unique value within the allowed number of attempts.
/// Occurs when all generated values for a given scope key have already been seen,
/// and the retry limit is exhausted.
/// </summary>
public class UniqueValueExhaustedException(string typeName, string key, int attempts)
    : QuickFuzzrException(BuildMessage(typeName, key, attempts))
{
    private static string BuildMessage(string typeName, string key, int attempts) =>
$@"Could not find a unique value of type {typeName} using key ""{key}"", after {attempts} {(attempts == 1 ? "attempt" : "attempts")}.

Possible solutions:
- Increase the retry limit globally: Configr.RetryLimit(256)
- Increase it locally: .Unique(""{key}"", 256)
- Widen the value space (add more options or relax filters)
- Use a deterministic unique source (Counter for instance)
- Use a different uniqueness scope key to reset tracking
- Use a fallback: fuzzr.Unique(values).WithDefault()
";
}