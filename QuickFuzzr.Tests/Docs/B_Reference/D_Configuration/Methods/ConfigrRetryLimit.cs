using QuickFuzzr.Tests._Tools;
using QuickFuzzr.UnderTheHood;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr.RetryLimit(int limit)")]
public class ConfigrRetryLimit
{
    [DocUsage]
    [DocExample(typeof(ConfigrRetryLimit), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr.RetryLimit(256);
    }

    [Fact]
    [DocContent("- Sets the global retry limit used by fuzzrs.")]
    public void SetsPropertyOnState()
    {
        var state = new State();
        Assert.Equal(64, state.RetryLimit);
        GetConfig()(state);
        Assert.Equal(256, state.RetryLimit);
    }

    [Fact]
    [DocContent("- Throws when trying to set limit to a value lesser than 1.")]
    public void Minimum_Is_One()
    {
        Configr.RetryLimit(1).Generate(); // works
        var ex = Assert.Throws<RetryLimitOutOfRangeException>(() => Configr.RetryLimit(0).Generate());
    }

    [Fact]
    [DocContent("- Throws when trying to set limit to a value greater than 1024.")]
    [DocExample(typeof(ConfigrRetryLimit), nameof(Maximum_Is_1024_Message), "text")]
    public void Minimum_Is_1024()
    {
        Configr.RetryLimit(1024).Generate(); // works
        var ex = Assert.Throws<RetryLimitOutOfRangeException>(() => Configr.RetryLimit(1025).Generate());
        Assert.Equal(Maximum_Is_1024_Message(), ex.Message);
    }

    [CodeSnippet]
    [CodeRemove("@\"")]
    [CodeRemove("\";")]
    private static string Maximum_Is_1024_Message() =>
@"Invalid retry limit value: 1025

Allowed range: 1-1024

Possible solutions:
- Use a value within the allowed range
- Check for unintended configuration overrides
- If you need more, consider revising your fuzzr logic instead of increasing the limit
";
}
