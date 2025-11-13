using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.C_ExtensionMethods.Methods;

[DocFile]
[DocFileCodeHeader("ExtFuzzr.WithDefault(this FuzzrOf<T> fuzzr, T def = default)")]
[DocColumn(FuzzrExtensionMethods.Columns.Description, "Returns def instead of throwing when the underlying fuzzr fails due to empty choices.")]
public class ExtFuzzrWithDefault
{
    [Fact]
    [DocContent("Returns a default value when the underlying fuzzr fails due to empty choices.")]
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
        Assert.Contains(value, new[] { 1, 2, 3 });
    }
}

