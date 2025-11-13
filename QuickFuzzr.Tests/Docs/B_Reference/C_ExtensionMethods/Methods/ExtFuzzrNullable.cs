using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.C_ExtensionMethods.Methods;

[DocFile]
[DocFileCodeHeader("ExtFuzzr.Nullable(this FuzzrOf<T> fuzzr)")]
[DocColumn(FuzzrExtensionMethods.Columns.Description, "Converts a non-nullable value-type fuzzr into a nullable one with a default 20% null probability.")]
public class ExtFuzzrNullable
{
    [DocContent("Wraps a value type fuzzr to sometimes yield null values.")]
    [Fact]
    [DocUsage]
    public void Can_Produce_Null_And_NotNull()
    {
        CheckIf.GeneratesNullAndNotNull(Fuzzr.Int().Nullable());
    }
}
