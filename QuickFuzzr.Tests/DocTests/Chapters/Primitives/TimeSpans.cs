using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Primitives;

[DocContent("Use `Fuzz.TimeSpan()`.")]
public class TimeSpans
{
	[Fact]
	[DocContent("- The overload `Fuzz.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.")]
	public void OverloadRange()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(Fuzz.TimeSpan(5),
			("Ticks >= 1", a => a.Ticks >= 1), ("Ticks < 5", a => a.Ticks < 5));
	}

	[Fact]
	[DocContent("- The default generator is (max = 1000).")]
	public void GeneratesValuesBetweenOneIncludedAndThousandExcluded()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(Fuzz.TimeSpan(),
			("Ticks >= 1", a => a.Ticks >= 1), ("Ticks < 1000", a => a.Ticks < 1000));
	}

	[Fact]
	[DocContent("- Can be made to return `TimeSpan?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		CheckIf.GeneratesNullAndNotNull(Fuzz.TimeSpan().Nullable());
	}

	[Fact]
	[DocContent("- `TimeSpan` is automatically detected and generated for object properties.")]
	public void Property()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(
			Fuzz.One<SomeThingToGenerate>().Select(a => a.AProperty),
			("not zero", a => a.Ticks != 0));
	}

	[Fact]
	[DocContent("- `TimeSpan?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		CheckIf.GeneratesNullAndNotNull(
			Fuzz.One<SomeThingToGenerate>().Select(a => a.ANullableProperty));
	}

	public class SomeThingToGenerate
	{
		public TimeSpan AProperty { get; set; }
		public TimeSpan? ANullableProperty { get; set; }
	}
}