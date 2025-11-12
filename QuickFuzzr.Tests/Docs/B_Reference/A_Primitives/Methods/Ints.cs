using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.A_Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Int()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces random integers (default 1-100).")]
public class Ints
{
	[Fact]
	[DocContent("- The overload `Fuzzr.Int(int min, int max)` generates an int greater than or equal to `min` and less than `max`.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Int(1, 5),
			("value >= 1", a => a >= 1), ("value < 5", a => a < 5));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
		=> Assert.Throws<ArgumentException>(() => Fuzzr.Int(1, 0).Generate());

	[Fact]
	[DocContent("- The default fuzzr is (min = 1, max = 100).")]
	public void DefaultFuzzr()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Int(),
			("value >= 1", a => a >= 1), ("value < 100", a => a < 100));

	[Fact]
	[DocContent("- Can be made to return `int?` using the `.Nullable()` combinator.")]
	public void Nullable()
		=> CheckIf.GeneratesNullAndNotNull(Fuzzr.Int().Nullable());

	[Fact]
	[DocContent("- `int` is automatically detected and generated for object properties.")]
	public void Property()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<int>>(),
			("value != 0", a => a.Value != 0));

	[Fact]
	[DocContent("- `int?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
		=> CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<PrimitivesBag<int>>().Select(a => a.NullableValue));
}