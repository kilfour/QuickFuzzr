using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileHeader("Fuzzr.OneOf&lt;T&gt;(params &lt;T&gt;[] values)")]
[DocColumn(Fuzzing.Columns.Description, "Randomly selects one of the provided values.")]
[DocContent("Creates a generator that randomly selects one value or generator from the provided options.")]
public class FuzzrOneOf
{
    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<string> GetFuzzr()
    {
        return Fuzzr.OneOf("a", "b", "c");
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(FuzzrOneOf), nameof(GetFuzzr))]
    public void Example()
    {
        var result = GetFuzzr().Generate(42);
        Assert.Equal("c", result);
    }

    [Fact]
    [DocContent("- Selection is uniform unless weights are specified (see below).")]
    public void Distribution()
    {
        var result = GetFuzzr().Many(100).Generate(1);
        Assert.Equal(36, result.Count(a => a == "a"));
        Assert.Equal(30, result.Count(a => a == "b"));
        Assert.Equal(34, result.Count(a => a == "c"));
        result = GetFuzzr().Many(100).Generate(2);
        Assert.Equal(35, result.Count(a => a == "a"));
        Assert.Equal(32, result.Count(a => a == "b"));
        Assert.Equal(33, result.Count(a => a == "c"));
    }

    [Fact]
    [DocContent("\n**Overloads:**")]
    [DocContent("- `Fuzzr.OneOf(IEnumerable<T> values)`:")]
    [DocContent("  Same as above, but accepts any enumerable source.")]
    public void Enumerable_Example()
    {
        IEnumerable<string> list = ["a", "b", "c"];
        var result = Fuzzr.OneOf(list).Generate(42);
        Assert.Equal("c", result);
    }

    [Fact]
    [DocContent("- `Fuzzr.OneOf(params FuzzrOf<T>[] generators)`:")]
    [DocContent("  Randomly selects and executes one of the provided generators.")]
    public void Generators_Example()
    {
        var result = Fuzzr.OneOf(Fuzzr.Constant("a"), Fuzzr.Constant("b"), Fuzzr.Constant("c")).Generate(42);
        Assert.Equal("c", result);
    }

    [CodeSnippet]
    [CodeRemove("return")]
    private static FuzzrOf<string> Weights_Example_GetFuzzr()
    {
        return Fuzzr.OneOf((1, "a"), (2, "b"), (3, "c"));
    }

    [Fact]
    [DocContent("- `Fuzzr.OneOf(params (int Weight, T Value)[] weightedValues)`:")]
    [DocContent("  Selects a value using weighted probability. The higher the weight, the more likely the value is to be chosen.")]
    [DocExample(typeof(FuzzrOneOf), nameof(Weights_Example_GetFuzzr))]
    public void Weights_Example()
    {
        var fuzzr = Weights_Example_GetFuzzr().Many(30);

        var result = fuzzr.Generate(1);
        Assert.Equal(4, result.Count(a => a == "a"));
        Assert.Equal(8, result.Count(a => a == "b"));
        Assert.Equal(18, result.Count(a => a == "c"));

        result = fuzzr.Generate(2);
        Assert.Equal(6, result.Count(a => a == "a"));
        Assert.Equal(11, result.Count(a => a == "b"));
        Assert.Equal(13, result.Count(a => a == "c"));
    }

    [Fact]
    [DocContent("- `Fuzzr.OneOf(params (int Weight, FuzzrOf<T> Generator)[] weightedGenerators)`:")]
    [DocContent("  Like the weighted values overload, but applies weights to generators.")]
    public void Weights_Fuzzrs()
    {
        var fuzzr =
            Fuzzr.OneOf(
                (1, Fuzzr.Constant("a")),
                (2, Fuzzr.Constant("b")),
                (3, Fuzzr.Constant("c")))
            .Many(30);

        var result = fuzzr.Generate(1);
        Assert.Equal(4, result.Count(a => a == "a"));
        Assert.Equal(8, result.Count(a => a == "b"));
        Assert.Equal(18, result.Count(a => a == "c"));

        result = fuzzr.Generate(2);
        Assert.Equal(6, result.Count(a => a == "a"));
        Assert.Equal(11, result.Count(a => a == "b"));
        Assert.Equal(13, result.Count(a => a == "c"));
    }

    [Fact]
    [DocContent("\n**Exceptions:**")]
    [DocContent("  - `OneOfEmptyOptionsException`: When trying to choose from an empty collection.")]
    public void Collection_Is_Empty()
    {
        List<Person> list = [];
        var ex = Assert.Throws<OneOfEmptyOptionsException>(() => Fuzzr.OneOf((IEnumerable<Person>)list).Generate());
        Assert.Equal(Collection_Is_Empty_Message(), ex.Message);
    }

    private static string Collection_Is_Empty_Message() =>
@"Fuzzr.OneOf<Person> cannot select from an empty sequence.

Possible solutions:
• Provide at least one option (ensure the sequence is non-empty).
• Use a fallback: Fuzzr.OneOf(values).WithDefault()
• Guard upstream: values.Any() ? Fuzzr.OneOf(values) : Fuzzr.Constant(default!).
";

    // Throws_When_Total_Weight_Not_Positive()
    [Fact]
    [DocContent("  - `NegativeWeightException`: When one or more weights are negative.")]
    public void Non_Positive_Weight()
    {
        var ex = Assert.Throws<NegativeWeightException>(() => Fuzzr.OneOf((-1, "a"), (2, "a")));
        Assert.Equal(Non_Positive_Weight_Message(), ex.Message);
    }

    private static string Non_Positive_Weight_Message() =>
@"Fuzzr.OneOf<String> cannot have negative weights.

Possible solutions:
• Ensure all weights are non-negative.
• Set the weight to 0 to disable a branch without removing it.
";


    [Fact]
    [DocContent("  - `ZeroTotalWeightException`: When the total of all weights is zero or negative.")]
    public void Total_Weight()
    {
        var ex = Assert.Throws<ZeroTotalWeightException>(() => Fuzzr.OneOf((0, "a")));
        Assert.Equal(Total_Weight_Message(), ex.Message);
    }

    private static string Total_Weight_Message() =>
@"Fuzzr.OneOf<String> cannot have a total weight of zero or less.

Possible solutions:
• Ensure at least one option has a positive weight.
• Use zero weight entries only to disable specific options.
• Adjust weights to ensure the total is greater than zero.
";

    [Fact]
    [DocContent("  - `ArgumentNullException`: When the provided sequence is null.")]
    public void Null_Values_Argument()
    {
        IEnumerable<string> list = null!;
        var ex = Assert.Throws<ArgumentNullException>(() => Fuzzr.OneOf(list).Generate());
        Assert.Equal(Null_Values_Argument_Message(), ex.Message);
    }

    private static string Null_Values_Argument_Message() =>
@"The sequence passed to Fuzzr.OneOf<String>(...) is null.

Possible solutions:
• Pass a non-null IEnumerable<T> (e.g. an empty array if you're building it later).
• If the sequence may be empty, use .WithDefault().
 (Parameter 'values')";
}

