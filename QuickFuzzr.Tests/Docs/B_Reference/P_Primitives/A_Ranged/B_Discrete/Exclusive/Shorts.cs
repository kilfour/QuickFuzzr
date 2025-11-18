using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.B_Discrete.Exclusive;

[DocContent("Use `Fuzzr.Short()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random 16-bit integers (default 1-100).")]
[DocContent("- **Default Range:** min = 1, max = 100).")]
public class Shorts : RangedPrimitive<short>
{
	protected override FuzzrOf<short> CreateFuzzr() => Fuzzr.Short();
	protected override FuzzrOf<short> CreateRangedFuzzr(short min, short max) => Fuzzr.Short(min, max);
	protected override (short Min, short Max) DefaultRange => (short.MinValue, short.MaxValue);
	protected override (short Min, short Max) ExampleRange => (5, 7);
	protected override (short Min, short Max) MinimalRange => (0, 1);
	protected override short GetUpperBoundarySample(short min, short max) => (short)(max - 1);
}