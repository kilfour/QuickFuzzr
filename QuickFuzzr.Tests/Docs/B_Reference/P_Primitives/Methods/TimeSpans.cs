using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocFileHeader("TimeSpans")]
[DocContent("Use `Fuzzr.TimeSpan()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random durations up to 1000 ticks by default.")]
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
	[DocContent("- The default fuzzr is (max = 1000).")]
	public void GeneratesValuesBetweenOneIncludedAndThousandExcluded()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.TimeSpan(),
			("Ticks >= 1", a => a.Ticks >= 1), ("Ticks < 1000", a => a.Ticks < 1000));
	}

	[Fact]
	public void Nullable()
	{
		CheckIf.GeneratesNullAndNotNull(Fuzzr.TimeSpan().Nullable());
	}

	[Fact]
	public void Property()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(
			Fuzzr.One<SomeThingToGenerate>().Select(a => a.AProperty),
			("not zero", a => a.Ticks != 0));
	}

	[Fact]
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