using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.A_Continuous;


[DocContent("Use `Fuzzr.Decimal()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random decimal numbers (default 1-100).")]
[DocContent("**Default Range and Precision:** min = 1, max = 100, precision = 2).")]
[DocContent("Apart from the usual ranged primitive min/max overload, `Fuzzr.Decimal()` adds two more allowing the user to specify a precision.")]
public class Decimals : RangedPrimitive<decimal>
{
	protected override FuzzrOf<decimal> CreateFuzzr() => Fuzzr.Decimal();
	protected override FuzzrOf<decimal> CreateRangedFuzzr(decimal min, decimal max) => Fuzzr.Decimal(min, max);
	protected override (decimal Min, decimal Max) DefaultRange => (decimal.MinValue, decimal.MaxValue);
	protected override (decimal Min, decimal Max) ExampleRange => (5, 7);
	protected override (decimal Min, decimal Max) MinimalRange => (0, 1);
	protected override bool CheckExactBoundaries => false;

	[Fact]
	public void DefaultFuzzrPrecision()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Decimal(),
			("precision <= 2", a => GetPrecision(a) <= 2));

	private static int GetPrecision(decimal value) => (decimal.GetBits(value)[3] >> 16) & 0x7F;

	[Fact]
	[DocContent("- The overload `Decimal(int precision)` generates a decimal with up to `precision` decimal places.")]
	public void Precision()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Decimal(1),
			("precision <= 1", a => GetPrecision(a) <= 1));
	[Fact]
	[DocContent("- Throws an `ArgumentException` when `precision` < `0`.")]
	public void DecimalPrecision_Throws()
		=> Assert.Throws<ArgumentOutOfRangeException>(() => Fuzzr.Decimal(-1).Generate());

	[Fact]
	public void PrecisionShouldBeGenerated()
		=> CheckIf.GeneratedValuesShouldEventuallySatisfyAll(Fuzzr.Decimal(1),
			("precision == 1", a => GetPrecision(a) == 1));

	[Fact]
	[DocContent("- The overload `Decimal(decimal min, decimal max, int precision)` generates a decimal in the range [min, max) (min inclusive, max exclusive), with up to `precision` decimal places.")]
	public void MinMaxPrecision()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Decimal(1, 5, 1),
			("value >= 1", a => a >= 1), ("value < 5", a => a < 5),
			("precision <= 1", a => GetPrecision(a) <= 1));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `precision` < `0`.")]
	public void MinMaxDecimalPrecision_Throws()
		=> Assert.Throws<ArgumentOutOfRangeException>(() => Fuzzr.Decimal(1, 2, -1));

}
