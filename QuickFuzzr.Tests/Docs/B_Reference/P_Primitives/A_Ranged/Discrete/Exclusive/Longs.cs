using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.Discrete.Exclusive;

[DocFile]
[DocContent("Use `Fuzzr.Long()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random 64-bit longegers (default 1-100).")]
[DocContent("- **Default:** min = 1, max = 100).")]
public class Longs : RangedPrimitive<long>
{
	protected override FuzzrOf<long> CreateFuzzr() => Fuzzr.Long();
	protected override FuzzrOf<long> CreateRangedFuzzr(long min, long max) => Fuzzr.Long(min, max);
	protected override (long Min, long Max) DefaultRange => (long.MinValue, long.MaxValue);
	protected override (long Min, long Max) ExampleRange => (5, 7);
	protected override (long Min, long Max) MinimalRange => (0, 1);
	protected override long GetUpperBoundarySample(long min, long max) => max - 1;
}