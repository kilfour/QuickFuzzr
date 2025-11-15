namespace QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

/// <summary>
/// Thrown when a retry limit value falls outside the allowed range.
/// Occurs when configuring <c>Configr.RetryLimit(...)</c> with a value below 1
/// or above the internal hard cap (1024).
/// </summary>
public class RetryLimitOutOfRangeException : QuickFuzzrException
{
    public RetryLimitOutOfRangeException(int limit)
        : base(BuildMessage(limit)) { }

    private static string BuildMessage(int limit) =>
$@"Invalid retry limit value: {limit}

Allowed range: 1-1024

Possible solutions:
- Use a value within the allowed range
- Check for unintended configuration overrides
- If you need more, consider revising your Fuzzr logic instead of increasing the limit
";
}
