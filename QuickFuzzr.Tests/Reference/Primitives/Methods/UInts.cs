using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.Primitives.Methods;

[DocFile]
[DocFileHeader("UInts")]
[DocContent("Use `Fuzzr.UInt()`.")]
public class UInts
{
	[Fact]
	[DocContent("- The overload `Fuzzr.UInt(uint min, uint max)` generates an uint greater than or equal to `min` and less than `max`.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.UInt(1, 5),
			("value >= 1", a => a >= 1), ("value < 5", a => a < 5));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
		=> Assert.Throws<ArgumentException>(() => Fuzzr.UInt(1, 0).Generate());

	[Fact]
	[DocContent("- The default generator is (min = 1, max = 100).")]
	public void DefaultGenerator()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.UInt(),
			("value >= 1", a => a >= 1), ("value < 100", a => a < 100));

	[Fact]
	[DocContent("- Can be made to return `uint?` using the `.Nullable()` combinator.")]
	public void Nullable()
		=> CheckIf.GeneratesNullAndNotNull(Fuzzr.UInt().Nullable());

	[Fact]
	[DocContent("- `uint` is automatically detected and generated for object properties.")]
	public void Property()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<uint>>(),
			("value != 0", a => a.Value != 0));

	[Fact]
	[DocContent("- `uint?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
		=> CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<PrimitivesBag<uint>>().Select(a => a.NullableValue));
}