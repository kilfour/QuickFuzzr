using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.B_Discrete.Exclusive;

[DocFileHeader("UShorts")]
[DocContent("Use `Fuzzr.UShort()`.")]
[DocContent("- **Default Range:** min = 1, max = 100).")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces unsigned 16-bit integers (default 1-100).")]
public class UShorts : RangedPrimitive<ushort>
{
	protected override FuzzrOf<ushort> CreateFuzzr() => Fuzzr.UShort();
	protected override FuzzrOf<ushort> CreateRangedFuzzr(ushort min, ushort max) => Fuzzr.UShort(min, max);
	protected override (ushort Min, ushort Max) DefaultRange => (ushort.MinValue, ushort.MaxValue);
	protected override (ushort Min, ushort Max) ExampleRange => (5, 7);
	protected override (ushort Min, ushort Max) MinimalRange => (0, 1);
	protected override bool CheckExactBoundaries => false;
}