using QuickFuzzr.Strings;
using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileHeader("Mutate")]
[DocColumn(FuzzrExtensionMethods.Columns.Description,
    "Chooses a mutation strategy and applies it a specified number of times.")]
[DocContent("Derives a new Fuzzr by transforming an existing Fuzzr. For each generated value, one mutation strategy is chosen and repeated, strategies are alternatives, not a pipeline. A mutation transforms a FuzzrOf<T> into another FuzzrOf<T>, so it can use Select, LINQ, or other generator extensions.")]
[DocSignature("fuzzr.Mutate((mutation, times), ...)")]
[DocSignature("fuzzr.Mutate((mutation, min, max), ...)")]
[DocContent("Supply mutations as (mutation, times) tuples for fixed repetition or (mutation, min, max) tuples for ranged repetition. Each mutation is a function from FuzzrOf<T> to FuzzrOf<T>.")]
public class ExtFuzzrMutate
{
    [CodeSnippet]
    private static FuzzrOf<int> Once_Example()
    {
        return Fuzzr.Constant(10).Mutate((source => source.Select(value => value + 1), 1));
        // Always generates 11: the transformation is applied once.
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(ExtFuzzrMutate), nameof(Once_Example))]
    public void Applies_A_Mutation_Once()
    {
        Assert.Equal(11, Once_Example().Generate(42));
    }

    [CodeSnippet]
    private static FuzzrOf<int> Fixed_Count_Example()
    {
        return Fuzzr.Constant(10).Mutate(
            (source => source.Select(value => value + 1), 3));
        // Always generates 13: the same transformation is applied three times.
    }

    [Fact]
    [DocContent("A (mutation, times) tuple repeats that transformation exactly times times. Repetition composes transformations; it does not produce a collection of independently mutated values.")]
    [DocExample(typeof(ExtFuzzrMutate), nameof(Fixed_Count_Example))]
    public void Repeats_A_Mutation_Exactly_The_Requested_Number_Of_Times()
    {
        Assert.Equal(13, Fixed_Count_Example().Generate(42));
    }

    [CodeSnippet]
    private static FuzzrOf<int> Range_Example()
    {
        return Fuzzr.Constant(10).Mutate(
            (source => source.Select(value => value + 1), 1, 4));
        // Generates 11, 12, or 13: Min is inclusive; Max is exclusive.
    }

    [Fact]
    [DocContent("A (mutation, min, max) tuple draws a repetition count for each generated value. Bounds follow Fuzzr.Int: min is inclusive and max is exclusive; equal bounds give that exact count. Use nonnegative counts. A fixed count of zero leaves the source unchanged.")]
    [DocExample(typeof(ExtFuzzrMutate), nameof(Range_Example))]
    public void Repetition_Range_Has_An_Exclusive_Upper_Bound()
    {
        var values = Enumerable.Range(0, 100)
            .Select(seed => Range_Example().Generate(seed)).ToArray();
        Assert.All(values, value => Assert.InRange(value, 11, 13));
        Assert.Equal(new[] { 11, 12, 13 }, values.Distinct().OrderBy(value => value));
    }

    [CodeSnippet]
    private static FuzzrOf<string> Alternatives_Example()
    {
        return Fuzzr.Constant("x").Mutate(
            (source => source.Select(text => text + "!"), 2),
            (source => source.Select(text => text + "?"), 2));
        // Generates "x!!" or "x??", never "x!?" or "x?!".
    }

    [Fact]
    [DocContent("When several mutations are supplied, each generated value uses one chosen strategy for all its repetitions. To compose different strategies in sequence, chain separate Mutate calls.")]
    [DocExample(typeof(ExtFuzzrMutate), nameof(Alternatives_Example))]
    public void Chooses_One_Strategy_And_Repeats_It_Without_Mixing()
    {
        var values = Enumerable.Range(0, 100)
            .Select(seed => Alternatives_Example().Generate(seed)).ToArray();
        Assert.All(values, value => Assert.Contains(value, new[] { "x!!", "x??" }));
        Assert.Equal(2, values.Distinct().Count());
    }

    [CodeSnippet]
    private static FuzzrOf<string> String_Example()
    {
        return Fuzzr.Constant("#A1B2C3").Mutate(
            (source => source.InsertOneOf(' ', '\t'), 1));
        // Adds one space or tab at a generated position, including either end.
        // InsertOneOf requires using QuickFuzzr.Strings.
    }

    [Fact]
    [DocContent("Mutations can introduce variations around a known starting value. They do not guarantee invalid input, or even a changed value: those properties depend on the transformation and the domain. In this example insertion guarantees one added character, but whether whitespace is allowed belongs to the consuming test.")]
    [DocExample(typeof(ExtFuzzrMutate), nameof(String_Example))]
    public void Can_Use_A_String_Generator_Transformation()
    {
        foreach (var seed in Enumerable.Range(0, 100))
        {
            var value = String_Example().Generate(seed);
            Assert.Equal(8, value.Length);
            Assert.Single(value, character => character is ' ' or '\t');
            Assert.Equal("#A1B2C3", value.Replace(" ", "").Replace("\t", ""));
        }
    }

    [Fact]
    public void Equal_Bounds_Repeat_Exactly_That_Count()
    {
        var fuzzr = Fuzzr.Constant(10).Mutate(
            (source => source.Select(value => value + 1), 3, 3));
        Assert.Equal(13, fuzzr.Generate(42));
    }

    [Fact]
    public void Zero_Repetitions_Preserve_The_Source()
    {
        var fuzzr = Fuzzr.Constant(10).Mutate(
            (source => source.Select(value => value + 1), 0));
        Assert.Equal(10, fuzzr.Generate(42));
    }

    [Fact]
    public void Separate_Mutate_Calls_Compose_Strategies()
    {
        var fuzzr = Fuzzr.Constant("x")
            .Mutate((source => source.Select(text => text + "!"), 2))
            .Mutate((source => source.Select(text => text + "?"), 2));
        Assert.Equal("x!!??", fuzzr.Generate(42));
    }
}
