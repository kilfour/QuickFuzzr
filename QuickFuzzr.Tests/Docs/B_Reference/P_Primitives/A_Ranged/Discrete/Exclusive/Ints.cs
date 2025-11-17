using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.Discrete.Exclusive;

[DocFile]
[DocContent("Use `Fuzzr.Int()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces random integers (default 1-100).")]
[DocContent("- **Default:** min = 1, max = 100).")]
public class Ints : RangedPrimitive<int>
{
	protected override FuzzrOf<int> CreateFuzzr() => Fuzzr.Int();
	protected override FuzzrOf<int> CreateRangedFuzzr(int min, int max) => Fuzzr.Int(min, max);
	protected override (int Min, int Max) DefaultRange => (int.MinValue, int.MaxValue);
	protected override (int Min, int Max) ExampleRange => (5, 7);
	protected override (int Min, int Max) MinimalRange => (0, 1);
	protected override int GetUpperBoundarySample(int min, int max) => max - 1;
}