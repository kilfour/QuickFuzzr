using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileHeader("ToArray")]
[DocColumn(FuzzrExtensionMethods.Columns.Description,
"Creates a Fuzzr that materializes the sequence produced by the source Fuzzr as an `Array<T>`.")]
[DocContent(
@"Creates a Fuzzr that materializes the sequence produced by the source Fuzzr as an `Array<T>`.  
This is a convenience method.
The equivalent behavior can be expressed with LINQ Select, but it removes boilerplate.")]
[DocSignature("ExtFuzzr.ToArray(this FuzzrOf<T> fuzzr)")]
public class ExtFuzzrToArray
{
    [CodeSnippet]
    [CodeRemove("42")]
    private static FuzzrOf<int[]> Usage() =>
        Fuzzr.Int().Many(5).ToArray();

    [Fact]
    [DocUsage]
    [DocExample(typeof(ExtFuzzrToList), nameof(Usage))]
    public void ToList_Materializes()
        => Assert.IsType<int[]>(Usage().Generate());
}

