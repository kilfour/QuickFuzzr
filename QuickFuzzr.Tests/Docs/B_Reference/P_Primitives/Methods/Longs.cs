using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Long()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random 64-bit integers (default 1-100).")]
public class Longs
{
	[Fact]
	[DocContent("- The overload `Fuzzr.Long(long min, long max)` generates a long greater than or equal to `min` and less than `max`.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Long(1, 5),
			("value >= 1", a => a >= 1), ("value < 5", a => a < 5));

	[Fact]
	public void MinMaxSame()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Long(1, 1),
			("value == 1", a => a == 1));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws() => Assert.Throws<ArgumentException>(() => Fuzzr.Long(1, 0).Generate());

	[Fact]
	[DocContent("- **Default:** min = 1, max = 100).")]
	public void DefaultFuzzr()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Long(),
			("value >= 1", a => a >= 1), ("value < 100", a => a < 100));

	[Fact]
	public void Nullable()
		=> CheckIf.GeneratesNullAndNotNull(Fuzzr.Long().Nullable());

	[Fact]
	public void Property()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<long>>(),
			("value != 0", a => a.Value != 0));

	[Fact]
	public void NullableProperty()
		=> CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<PrimitivesBag<long>>().Select(a => a.NullableValue));
}