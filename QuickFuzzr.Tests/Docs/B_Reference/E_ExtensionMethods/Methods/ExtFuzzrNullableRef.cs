using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.E_ExtensionMethods.Methods;

[DocFile]
[DocFileHeader("NullableRef")]
[DocColumn(FuzzrExtensionMethods.Columns.Description,
"Wraps a reference-type fuzzr to sometimes return null (default 20% chance).")]
[DocContent("Wraps a reference type fuzzr to sometimes yield null values.")]
[DocSignature("ExtFuzzr.NullableRef(this FuzzrOf<T> fuzzr)")]
public class ExtFuzzrNullableRef
{
    [Fact]
    public void Can_Produce_Null_And_NotNull()
    {
        CheckIf.GeneratesNullAndNotNull(Fuzzr.String().NullableRef());
    }

    // TODO [Fact]
    // TODO [DocUsage]
    // TODO public void Can_Produce_Null_And_NotNull_With_Custom_Null_Probability()
}
