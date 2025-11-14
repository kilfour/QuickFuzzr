using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileCodeHeader("AsObject")]
[DocColumn(FuzzrExtensionMethods.Columns.Description, "Boxes generated values as `object` without modifying them.")]
[DocContent("Boxes generated values as object without altering them.")]
[DocSignature("ExtFuzzr.AsObject<T>(this FuzzrOf<T> fuzzr)")]
public class ExtFuzzrAsObject
{
    [CodeSnippet]
    [CodeRemove("return ")]
    private static FuzzrOf<object> Boxes_Value_Fuzzr()
    {
        return Fuzzr.Constant(42).AsObject();
        // Results in => 42
    }

    [Fact]
    [DocUsage]
    [DocExample(typeof(ExtFuzzrAsObject), nameof(Boxes_Value_Fuzzr))]
    public void Boxes_Value()
    {
        var obj = Boxes_Value_Fuzzr().Generate();
        Assert.IsType<int>(obj);
        Assert.Equal(42, obj);
    }
}
