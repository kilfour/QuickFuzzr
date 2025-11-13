using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.C_ExtensionMethods.Methods;

[DocFile]
[DocFileHeader("NullableRef(this FuzzrOf<T> fuzzr)")]
[DocColumn(FuzzrExtensionMethods.Columns.Description, "Wraps a reference-type fuzzr to sometimes return null (default 20% chance).")]
public class ExtFuzzrNullableRef
{
    [Fact]
    [DocContent("Wraps a reference type fuzzr to sometimes yield null values.")]
    public void Can_Produce_Null_And_NotNull()
    {
        CheckIf.GeneratesNullAndNotNull(Fuzzr.String().NullableRef());
    }
}
