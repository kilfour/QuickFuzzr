using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Primitives;

[DocFile]
[DocContent("Use `Fuzzr.Long()`.")]
public class Longs
{
	[Fact]
	[DocContent("- The overload `Fuzzr.Long(long min, long max)` generates a long greater than or equal to `min` and less than `max`.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Long(1, 5),
			("value >= 1", a => a >= 1), ("value < 5", a => a < 5));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws() => Assert.Throws<ArgumentException>(() => Fuzzr.Long(1, 0).Generate());

	[Fact]
	[DocContent("- The default generator is (min = 1, max = 100).")]
	public void DefaultGenerator()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Long(),
			("value >= 1", a => a >= 1), ("value < 100", a => a < 100));

	[Fact]
	[DocContent("- Can be made to return `long?` using the `.Nullable()` combinator.")]
	public void Nullable()
		=> CheckIf.GeneratesNullAndNotNull(Fuzzr.Long().Nullable());

	[Fact]
	[DocContent("- `long` is automatically detected and generated for object properties.")]
	public void Property()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<long>>(),
			("value != 0", a => a.Value != 0));

	[Fact]
	[DocContent("- `long?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
		=> CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<PrimitivesBag<long>>().Select(a => a.NullableValue));
}