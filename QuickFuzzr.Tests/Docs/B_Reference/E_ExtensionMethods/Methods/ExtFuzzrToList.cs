using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileHeader("ToList")]
[DocColumn(FuzzrExtensionMethods.Columns.Description,
"Creates a Fuzzr that materializes the sequence produced by the source Fuzzr as a `List<T>`.")]
[DocContent(
@"Creates a Fuzzr that materializes the sequence produced by the source Fuzzr as a `List<T>`.  
This is a convenience method.
The equivalent behavior can be expressed with LINQ Select, but it removes boilerplate.")]
[DocSignature("ExtFuzzr.ToList(this FuzzrOf<T> fuzzr)")]
public class ExtFuzzrToList
{
    [CodeSnippet]
    private static FuzzrOf<List<int>> Usage() =>
        Fuzzr.Int().Many(5).ToList();

    [Fact]
    [DocUsage]
    [DocExample(typeof(ExtFuzzrToList), nameof(Usage))]
    public void ToList_Materializes()
        => Assert.IsType<List<int>>(Usage().Generate());
}

