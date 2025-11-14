using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileHeader("WithDefault")]
[DocColumn(FuzzrExtensionMethods.Columns.Description,
"Returns a default value when the underlying fuzzr fails due to empty choices.")]
[DocContent("Returns the (optionally) provided default value instead of throwing when the underlying fuzzr fails due to empty choices.")]
[DocSignature("ExtFuzzr.WithDefault(this FuzzrOf<T> fuzzr, T def = default)")]
public class ExtFuzzrWithDefault
{
    [Fact]
    public void Uses_Default_On_Empty_Choices()
    {
        var fuzzr = Fuzzr.OneOf(Array.Empty<int>()).WithDefault(42);
        Assert.Equal(42, fuzzr.Generate());
    }

    [Fact]
    public void Passes_Through_When_Choices_Present()
    {
        var fuzzr = Fuzzr.OneOf(1, 2, 3).WithDefault(42);
        var value = fuzzr.Generate(1);
        Assert.Equal(1, value);
    }
}

