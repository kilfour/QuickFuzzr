using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.B_Fuzzing.Methods;

[DocFile]
[DocColumn(Fuzzing.Columns.Description, "Returns the provided values in order, repeating when it reaches the end.")]
[DocContent(
@"Creates a Fuzzr that produces each provided value in order.  
After the final value, it starts again at the beginning.
")]
[DocSignature("Fuzzr.Sequence(params T[] values)")]
public class V_Sequence
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<int> Sequence_Example_Fuzzr()
    {
        return Fuzzr.Sequence(42, 43, 44);
        // Generate once results in => 42
        // second generate => 43
        // third generate => 44
        // fourth generate => 42
        // ...
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(V_Sequence), nameof(Sequence_Example_Fuzzr))]
    public void Generate_Once()
    {
        var result = Sequence_Example_Fuzzr().Many(7).Generate();
        Assert.Equal([42, 43, 44, 42, 43, 44, 42], result);
    }

    [Fact]
    [DocContent("- Sequence state resets between separate `Generate()` calls.")]
    public void Resets_Between_Runs()
    {
        var fuzzr = Sequence_Example_Fuzzr().Many(4);
        Assert.Equal([42, 43, 44, 42], fuzzr.Generate());
        Assert.Equal([42, 43, 44, 42], fuzzr.Generate());
    }

    [Fact]
    [DocContent("- The provided values are captured when the Fuzzr is created.")]
    public void Captures_Values()
    {
        int[] values = [1, 2];
        var fuzzr = Fuzzr.Sequence(values);
        values[0] = 99;

        Assert.Equal([1, 2], fuzzr.Many(2).Generate());
    }

    [Fact]
    [DocExceptions]
    [DocException("ArgumentNullException", "When the provided values are null.")]
    public void Null_Values_Throws()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Fuzzr.Sequence<int>(null!));
        Assert.Equal("Value cannot be null. (Parameter 'values')", ex.Message);
    }

    [Fact]
    [DocException("ArgumentException", "When no values are provided.")]
    public void Empty_Values_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() => Fuzzr.Sequence<int>());
        Assert.Equal(
            "The sequence must contain at least one value. (Parameter 'values')",
            ex.Message);
    }
}
