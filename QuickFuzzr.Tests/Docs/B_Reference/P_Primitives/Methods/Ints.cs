using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Int()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces random integers (default 1-100).")]
public class Ints : Primitive<int>
{
	[Fact]
	[DocContent("- The overload `Fuzzr.Int(int min, int max)` generates an int greater than or equal to `min` and less than `max`.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Int(1, 5),
			("value >= 1", a => a >= 1), ("value < 5", a => a < 5));

	[Fact]
	public void MinMaxSame()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Int(1, 1),
			("value == 1", a => a == 1));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
		=> Assert.Throws<ArgumentException>(() => Fuzzr.Int(1, 0).Generate());

	[Fact]
	[DocContent("- **Default:** min = 1, max = 100).")]
	public void DefaultFuzzr()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Int(),
			("value >= 1", a => a >= 1), ("value < 100", a => a < 100));

	protected override FuzzrOf<int> CreateFuzzr() => Fuzzr.Int();
}