using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.Tests.Reference.B_Fuzzing.Methods;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.D_Configuration.Methods;


[DocFile]
[DocFileHeader("Configr&lt;T&gt;.Depth(int min, int max)")]
[DocContent(
@"Configures depth constraints for type `T` to control recursive object graph generation. 
")]
public class ConfigrDepthT : ExplainMe<ConfigrDepthT>
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<Intent> GetConfig()
    {
        return Configr<Turtle>.Depth(2, 5);
        // Results in =>
        // Turtle { Down: { Down: { Down: { Down: null } } } }
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(ConfigrDepthT), nameof(GetConfig))]
    [DocContent(
@"Subsequent calls to `Fuzzr.One<T>()` will generate between 2 and 5 nested levels of `Turtle` instances,
depending on the random draw and available recursion budget.")]
    public void Example()
    {
        var fuzzr =
            from _ in GetConfig()
            from turtle in Fuzzr.One<Turtle>()
            select turtle;

        var result = fuzzr.Generate(43);
        Assert.NotNull(result);
        Assert.NotNull(result.Down);
        Assert.NotNull(result.Down.Down);
        Assert.NotNull(result.Down.Down.Down);
        Assert.Null(result.Down.Down.Down.Down);
    }

    //     Behavior:

    // min defines the minimum guaranteed nesting depth.

    // max defines the maximum possible nesting depth.

    // When the recursion counter reaches zero, QuickFuzzr yields null (or an empty collection for lists).

    // Depth is per type, not global — each recursive type manages its own budget.

    // Exceptions:

    // ArgumentOutOfRangeException: When min or max are negative.

    // ArgumentException: When min > max.
}