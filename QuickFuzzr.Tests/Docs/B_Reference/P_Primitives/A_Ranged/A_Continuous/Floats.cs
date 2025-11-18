using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.A_Continuous;


[DocContent("Use `Fuzzr.Float()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random single-precision numbers (default 1-100).")]
[DocContent("- **Default Range:** min = 1, max = 100).")]
public class Floats : RangedPrimitive<float>
{
	protected override FuzzrOf<float> CreateFuzzr() => Fuzzr.Float();
	protected override FuzzrOf<float> CreateRangedFuzzr(float min, float max) => Fuzzr.Float(min, max);
	protected override (float Min, float Max) DefaultRange => (float.MinValue, float.MaxValue);
	protected override (float Min, float Max) ExampleRange => (5, 7);
	protected override (float Min, float Max) MinimalRange => (0, 1);
	protected override bool CheckExactBoundaries => false;
}