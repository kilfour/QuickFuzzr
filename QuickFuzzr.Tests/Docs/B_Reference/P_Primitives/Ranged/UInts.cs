using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Ranged;

[DocFile]
[DocFileHeader("UInts")]
[DocContent("Use `Fuzzr.UInt()`.")]
[DocContent("- **Default:** min = 1, max = 100).")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces unsigned integers (default 1-100).")]
public class UInts : RangedPrimitive<uint>
{
	protected override FuzzrOf<uint> CreateFuzzr() => Fuzzr.UInt();
	protected override FuzzrOf<uint> CreateRangedFuzzr(uint min, uint max) => Fuzzr.UInt(min, max);
	protected override (uint Min, uint Max) DefaultRange => (uint.MinValue, uint.MaxValue);
	protected override (uint Min, uint Max) ExampleRange => (5, 7);
	protected override (uint Min, uint Max) MinimalRange => (0, 1);
	protected override uint GetUpperBoundarySample(uint min, uint max) => max - 1;
}