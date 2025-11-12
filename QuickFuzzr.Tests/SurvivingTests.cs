namespace QuickFuzzr.Tests;

public class SurvivingTests
{
    // -------------------------------------------------------------------------------------------
    [Fact(Skip = "apparantly not")]
    // `record` generation is also possible.")]
    public void CanGenerateRecords()
    {
        var fuzzr =
            from _ in Configr.EnablePropertyAccessFor(PropertyAccess.AllSetters)
            from __ in Configr.Primitive(Fuzzr.Constant(42))
            from record in Fuzzr.One<MyRecord>()
            select record;
        var result = fuzzr.Generate();
        Assert.Equal(42, result.Value);
    }

    public record MyRecord
    {
        public MyRecord() { }

        public MyRecord(int Value)
        {
            this.Value = Value;
        }

        public int Value { get; init; }
    }
    // -------------------------------------------------------------------------------------------
}