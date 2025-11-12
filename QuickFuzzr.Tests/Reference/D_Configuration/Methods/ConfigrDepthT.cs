using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
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
depending on the random draw and available recursion budget.  
Depth is per type, not global. Each recursive type manages its own budget.
")] // TODO: test different types with different depths
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

    [Fact]
    public void WithDepth_1_3()
    {
        var fuzzr =
            from _ in Configr<Turtle>.Depth(1, 3)
            from value in Fuzzr.One<Turtle>()
            select value;

        CheckIf.GeneratedValuesShouldEventuallySatisfyAll(
            fuzzr.Select(GetTurtleDepth),
            ("has depth 1", d => d == 1),
            ("has depth 2", d => d == 2),
            ("has depth 3", d => d == 3),
            ("no depth 4", d => d != 4)
        );
    }

    private static int GetTurtleDepth(Turtle turtle)
    {
        if (turtle.Down == null) return 1;
        if (turtle.Down.Down == null) return 2;
        if (turtle.Down.Down.Down == null) return 3;
        return 4;
    }

    [Fact]
    [DocExceptions]
    [DocException("ArgumentOutOfRangeException", "When min is negative.")]
    public void Min_Is_Negative()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Configr<Turtle>.Depth(-1, 3));
        Assert.Equal(Min_Is_Negative_Message(), ex.Message);
    }

    private static string Min_Is_Negative_Message() =>
@"Minimum depth must be non-negative for type Turtle. (Parameter 'min')";


    [Fact]
    [DocException("ArgumentOutOfRangeException", "When max is lesser than min")]
    public void Max_Is_Lesser_Than()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Configr<Turtle>.Depth(3, 1));
        Assert.Equal(Max_Is_Lesser_Than_Message(), ex.Message);
    }

    private static string Max_Is_Lesser_Than_Message() =>
@"Maximum depth must be greater than or equal to minimum depth for type Turtle. (Parameter 'max')";
}