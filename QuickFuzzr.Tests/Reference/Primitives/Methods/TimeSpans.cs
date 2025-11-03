using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Reference.Primitives.Methods;

[DocFile]
[DocFileHeader("TimeSpans")]
[DocContent("Use `Fuzzr.TimeSpan()`.")]
public class TimeSpans
{
	[Fact]
	[DocContent("- The overload `Fuzzr.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.")]
	public void OverloadRange()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.TimeSpan(5),
			("Ticks >= 1", a => a.Ticks >= 1), ("Ticks < 5", a => a.Ticks < 5));
	}

	[Fact]
	[DocContent("- The default generator is (max = 1000).")]
	public void GeneratesValuesBetweenOneIncludedAndThousandExcluded()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.TimeSpan(),
			("Ticks >= 1", a => a.Ticks >= 1), ("Ticks < 1000", a => a.Ticks < 1000));
	}

	[Fact]
	[DocContent("- Can be made to return `TimeSpan?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		CheckIf.GeneratesNullAndNotNull(Fuzzr.TimeSpan().Nullable());
	}

	[Fact]
	[DocContent("- `TimeSpan` is automatically detected and generated for object properties.")]
	public void Property()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(
			Fuzzr.One<SomeThingToGenerate>().Select(a => a.AProperty),
			("not zero", a => a.Ticks != 0));
	}

	[Fact]
	[DocContent("- `TimeSpan?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<SomeThingToGenerate>().Select(a => a.ANullableProperty));
	}

	public class SomeThingToGenerate
	{
		public TimeSpan AProperty { get; set; }
		public TimeSpan? ANullableProperty { get; set; }
	}
}