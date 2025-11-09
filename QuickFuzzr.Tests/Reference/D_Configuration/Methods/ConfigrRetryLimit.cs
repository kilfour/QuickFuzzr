using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr.RetryLimit(int limit)")]
public class ConfigrRetryLimit
{
    [DocContent("**Usage:**")]
    [DocExample(typeof(ConfigrRetryLimit), nameof(GetConfig))]
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr.RetryLimit(256);
    }

    [Fact]
    [DocContent("Sets the global retry limit used by generators.")]
    public void SetsPropertyOnState()
    {
        var state = new State();
        Assert.Equal(64, state.RetryLimit);
        GetConfig()(state);
        Assert.Equal(256, state.RetryLimit);
    }
}
