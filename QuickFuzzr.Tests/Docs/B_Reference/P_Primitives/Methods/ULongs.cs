using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocFileHeader("ULongs")]
[DocContent("Use `Fuzzr.ULong()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates unsigned 64-bit integers (default 1-100).")]
public class ULongs
{
	[Fact]
	[DocContent("- The overload `Fuzzr.ULong(ulong min, ulong max)` generates a ulong greater than or equal to `min` and less than `max`.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.ULong(1, 5),
			("value >= 1", a => a >= 1), ("value < 5", a => a < 5));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws() => Assert.Throws<ArgumentException>(() => Fuzzr.ULong(1, 0).Generate());

	[Fact]
	[DocContent("- **Default:** min = 1, max = 100).")]
	public void DefaultFuzzr()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.ULong(),
			("value >= 1", a => a >= 1), ("value < 100", a => a < 100));

	[Fact]
	public void Nullable()
		=> CheckIf.GeneratesNullAndNotNull(Fuzzr.ULong().Nullable());

	[Fact]
	public void Property()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<ulong>>(),
			("value != 0", a => a.Value != 0));

	[Fact]
	public void NullableProperty()
		=> CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<PrimitivesBag<ulong>>().Select(a => a.NullableValue));
}