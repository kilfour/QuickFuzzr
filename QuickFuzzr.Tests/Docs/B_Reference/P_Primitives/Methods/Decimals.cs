using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Decimal()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random decimal numbers (default 1-100).")]
public class Decimals
{
	[Fact]
	[DocContent(
@"
- The overload `Fuzzr.Decimal(decimal min, decimal max)` generates a decimal in the range [min, max) (min inclusive, max exclusive).")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Decimal(1, 5),
			("value >= 1", a => a >= 1), ("value < 5", a => a < 5));

	private static int GetPrecision(decimal value) => (decimal.GetBits(value)[3] >> 16) & 0x7F;

	[Fact]
	[DocContent("- The overload `Decimal(int precision)` generates a decimal with up to `precision` decimal places.")]
	public void Precision()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Decimal(1),
			("value >= 1", a => a >= 1), ("value < 100", a => a < 100),
			("precision <= 1", a => GetPrecision(a) <= 1));

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
	[DocContent("- When `min == max`, the fuzzr always returns that exact value.")]
	public void MinMaxEqual()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Decimal(42, 42),
			("== 42", a => a == 42));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzzr.Decimal(1, 0).Generate());
	}

	[Fact]
	[DocContent("- The default fuzzr is (min = 1, max = 100, precision = 2).")]
	public void DefaultFuzzr()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Decimal(),
			("value >= 1", a => a >= 1), ("value < 100", a => a < 100),
			("precision <= 2", a => GetPrecision(a) <= 2));

	[Fact]
	[DocContent("- Can be made to return `decimal?` using the `.Nullable()` combinator.")]
	public void Nullable()
		=> CheckIf.GeneratesNullAndNotNull(Fuzzr.Decimal().Nullable());

	[Fact]
	[DocContent("- `decimal` is automatically detected and generated for object properties.")]
	public void Property()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<decimal>>(),
			("value != 0", a => a.Value != 0));

	[Fact]
	[DocContent("- `decimal?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
		=> CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<PrimitivesBag<decimal>>().Select(a => a.NullableValue));
}
