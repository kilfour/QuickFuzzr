using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.T_Reference.A_Primitives.Methods;

[DocFile]
[DocFileHeader("UShorts")]
[DocContent("Use `Fuzzr.UShort()`.")]
public class UShorts
{
	[Fact]
	[DocContent("- The overload `Fuzzr.UShort(ushort min, ushort max)` generates a ushort greater than or equal to `min` and less than `max`.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.UShort(1, 5),
			("value >= 1", a => a >= 1), ("value < 5", a => a < 5));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void Throws()
		=> Assert.Throws<ArgumentException>(() => Fuzzr.UShort(1, 0).Generate());

	[Fact]
	[DocContent("- The default generator is (min = 1, max = 100).")]
	public void DefaultGenerator()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.UShort(),
			("value >= 1", a => a >= 1), ("value < 100", a => a < 100));

	[Fact]
	[DocContent("- Can be made to return `ushort?` using the `.Nullable()` combinator.")]
	public void Nullable()
		=> CheckIf.GeneratesNullAndNotNull(Fuzzr.UShort().Nullable());

	[Fact]
	[DocContent("- `ushort` is automatically detected and generated for object properties.")]
	public void Property()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<PrimitivesBag<ushort>>(),
			("value != 0", a => a.Value != 0));

	[Fact]
	[DocContent("- `ushort?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
		=> CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<PrimitivesBag<ushort>>().Select(a => a.NullableValue));
}