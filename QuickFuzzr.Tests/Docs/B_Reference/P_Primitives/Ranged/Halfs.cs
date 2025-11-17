using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Ranged;

[DocFile]
[DocContent("Use `Fuzzr.Half()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random 16-bit Halfing-point numbers (default 1-100).")]
[DocContent("- **Default:** min = (Half)1, max = (Half)100).")]
[DocContent("*Note:* Due to floatinging-point rounding, max may occasionally be produced.")]
public class Halfs : RangedPrimitive<Half>
{
    protected override FuzzrOf<Half> CreateFuzzr() => Fuzzr.Half();
    protected override FuzzrOf<Half> CreateRangedFuzzr(Half min, Half max) => Fuzzr.Half(min, max);
    protected override (Half Min, Half Max) DefaultRange => (Half.MinValue, Half.MaxValue);
    protected override (Half Min, Half Max) ExampleRange => ((Half)5, (Half)7);
    protected override (Half Min, Half Max) MinimalRange => ((Half)0, (Half)1);
    protected override bool UpperBoundExclusive => false; // due to the floating-point rounding
    protected override bool CheckExactBoundaries => false;

}