using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Ranged;

[DocFile]
[DocContent("Use `Fuzzr.Double()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random double-precision numbers (default 1-100).")]
[DocContent("- **Default:** min = 1, max = 100).")]
public class Doubles : RangedPrimitive<double>
{
	protected override FuzzrOf<double> CreateFuzzr() => Fuzzr.Double();
	protected override FuzzrOf<double> CreateRangedFuzzr(double min, double max) => Fuzzr.Double(min, max);
	protected override (double Min, double Max) DefaultRange => (double.MinValue, double.MaxValue);
	protected override (double Min, double Max) ExampleRange => (5, 7);
	protected override (double Min, double Max) MinimalRange => (0, 1);
	protected override bool CheckExactBoundaries => false;
}