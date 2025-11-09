using QuickFuzzr.Tests._Tools.Models;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.B_Fuzzing.Methods;

[DocFile]
[DocFileHeader("Fuzzr.OneOf&lt;T&gt;(...)")]
public class FuzzrOneOf
{
    // [DocContent("**Usage:**")]
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
}
