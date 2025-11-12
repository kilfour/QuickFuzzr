using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.C_ExtensionMethods.Methods;

[DocFile]
[DocFileHeader("ExtFuzzr.NullableRef(this FuzzrOf<T> fuzzr)")]
public class ExtFuzzrNullableRef
{
    [Fact]
    [DocContent("Wraps a reference type fuzzr to sometimes yield null values.")]
    public void Can_Produce_Null_And_NotNull()
    {
        CheckIf.GeneratesNullAndNotNull(Fuzzr.String().NullableRef());
    }
}
