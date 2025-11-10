using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileHeader("Fuzzr.OneOf&lt;T&gt;(...)")]
public class FuzzrOneOf
{
    // [DocUsage]
    // [DocExample(typeof(ConfigrTProperty), nameof(GetConfig))]
    // [CodeSnippet]
    // [CodeRemove("return")]
    // private static FuzzrOf<Intent> GetConfig()
    // {
    //     return Configr<Thing>.Property(s => s.Id, Fuzzr.Constant(42));
    // }

    [Fact]
    [DocContent("- Trying to choose from an empty collection throws an exception with the following message:")]
    [DocExample(typeof(FuzzrOneOf), nameof(Collection_Is_Empty_Message), "text")]
    public void Collection_Is_Empty()
    {
        List<Person> list = [];
        var ex = Assert.Throws<OneOfEmptyOptionsException>(() => Fuzzr.OneOf((IEnumerable<Person>)list).Generate());
        Assert.Equal(Collection_Is_Empty_Message(), ex.Message);
    }

    [CodeSnippet]
    [CodeRemove("@\"")]
    [CodeRemove("\";")]
    private static string Collection_Is_Empty_Message() =>
@"Fuzzr.OneOf<Person> cannot select from an empty sequence.

Possible solutions:
• Provide at least one option (ensure the sequence is non-empty).
• Use a fallback: Fuzzr.OneOf(values).WithDefault()
• Guard upstream: values.Any() ? Fuzzr.OneOf(values) : Fuzzr.Constant(default!).
";

    [Fact]
    [DocContent("- An overload exists that takes `params (int Weight, T Value)[] values` arguments in order to influence the distribution of generated values.")]
    public void Weights()
    {
        var result = Fuzzr.OneOf((1, "a"), (2, "b"), (3, "c")).Many(600).Generate(1);
        Assert.Equal(93, result.Count(a => a == "a"));
        Assert.Equal(199, result.Count(a => a == "b"));
        Assert.Equal(308, result.Count(a => a == "c"));
        result = Fuzzr.OneOf((1, "a"), (2, "b"), (3, "c")).Many(600).Generate(2);
        Assert.Equal(108, result.Count(a => a == "a"));
        Assert.Equal(221, result.Count(a => a == "b"));
        Assert.Equal(271, result.Count(a => a == "c"));
    }

    [Fact]
    [DocContent("- An overload exists that takes `params (int Weight, FuzzrOf<T> Generator)[] values` arguments in order to influence the distribution of generated values.")]
    public void Weights_Fuzzrs()
    {
        var result = Fuzzr.OneOf((1, Fuzzr.Constant("a")), (2, Fuzzr.Constant("b")), (3, Fuzzr.Constant("c"))).Many(600).Generate(1);
        Assert.Equal(93, result.Count(a => a == "a"));
        Assert.Equal(199, result.Count(a => a == "b"));
        Assert.Equal(308, result.Count(a => a == "c"));
        result = Fuzzr.OneOf((1, "a"), (2, "b"), (3, "c")).Many(600).Generate(2);
        Assert.Equal(108, result.Count(a => a == "a"));
        Assert.Equal(221, result.Count(a => a == "b"));
        Assert.Equal(271, result.Count(a => a == "c"));
    }
}
