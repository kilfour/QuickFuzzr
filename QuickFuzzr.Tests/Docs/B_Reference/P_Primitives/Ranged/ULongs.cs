using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Ranged;

[DocFile]
[DocFileHeader("ULongs")]
[DocContent("Use `Fuzzr.ULong()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates unsigned 64-bit integers (default 1-100).")]
[DocContent("- **Default:** min = 1, max = 100).")]
public class ULongs : RangedPrimitive<ulong>
{
	protected override FuzzrOf<ulong> CreateFuzzr() => Fuzzr.ULong();
	protected override FuzzrOf<ulong> CreateRangedFuzzr(ulong min, ulong max) => Fuzzr.ULong(min, max);
	protected override (ulong Min, ulong Max) DefaultRange => (ulong.MinValue, ulong.MaxValue);
	protected override (ulong Min, ulong Max) ExampleRange => (5, 7);
	protected override (ulong Min, ulong Max) MinimalRange => (0, 1);
	protected override ulong GetUpperBoundarySample(ulong min, ulong max) => max - 1;
}