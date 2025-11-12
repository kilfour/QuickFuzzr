using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.C_ExtensionMethods.Methods;

[DocFile]
[DocFileCodeHeader("ExtFuzzr.AsObject(this FuzzrOf<T> fuzzr)")]
public class ExtFuzzrAsObject
{

    [DocContent("Boxes generated values as object without altering them.")]
    [Fact]
    public void Boxes_Value()
    {
        var obj = Fuzzr.Constant(42).AsObject().Generate();
        Assert.IsType<int>(obj);
        Assert.Equal(42, (int)obj);
    }
}
