using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocFileHeader("TimeSpans")]
[DocContent("Use `Fuzzr.TimeSpan()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random durations up to 1000 ticks by default.")]
public class TimeSpans : Primitive<TimeSpan>
{
	protected override FuzzrOf<TimeSpan> CreateFuzzr() => Fuzzr.TimeSpan();

	[Fact]
	[DocContent("- The overload `Fuzzr.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.")]
	public void OverloadRange()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.TimeSpan(5),
			("Ticks >= 1", a => a.Ticks >= 1), ("Ticks < 5", a => a.Ticks < 5));
	}

	[Fact]
	[DocContent("- **Default:** max = 1000).")]
	public void GeneratesValuesBetweenOneIncludedAndThousandExcluded()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.TimeSpan(),
			("Ticks >= 1", a => a.Ticks >= 1), ("Ticks < 1000", a => a.Ticks < 1000));
	}
}