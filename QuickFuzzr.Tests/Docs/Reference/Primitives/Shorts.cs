using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Primitives;

[DocFile]
[DocContent("Use `Fuzzr.Short()`.")]
public class Shorts
{
	[Fact]
	[DocContent("- The overload `Fuzzr.Short(short min, short max)` generates a short higher or equal than min and lower than max.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Short(1, 5),
			("value >= 1", a => a >= 1), ("value < 5", a => a < 5));

	[Fact]
	[DocContent("- Throws an ArgumentException if min > max.")]
	public void Throws()
		=> Assert.Throws<ArgumentException>(() => Fuzzr.Short(1, 0).Generate());

	[Fact]
	[DocContent("- The default generator is (min = 1, max = 100).")]
	public void DefaultGenerator()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.Short(),
			("value >= 1", a => a >= 1), ("value < 100", a => a < 100));

	[Fact]
	[DocContent("- Can be made to return `short?` using the `.Nullable()` combinator.")]
	public void Nullable()
		=> CheckIf.GeneratesNullAndNotNull(Fuzzr.Short().Nullable());

	[Fact]
	[DocContent("- `short` is automatically detected and generated for object properties.")]
	public void Property()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<short>>(),
			("value != 0", a => a.Value != 0));

	[Fact]
	[DocContent("- `short?` is automatically detected and generated for object properties.")]
	public void NullableGenericProperty()
		=> CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<PrimitivesBag<short>>().Select(a => a.NullableValue));
}