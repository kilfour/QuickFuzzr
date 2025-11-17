using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.Discrete.Exclusive;

[DocFile]
[DocFileHeader("TimeSpans")]
[DocContent("Use `Fuzzr.TimeSpan()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Generates random durations up to 1000 ticks by default.")]
[DocContent("- **Default:** max = 1000).")]
public class TimeSpans : RangedPrimitive<TimeSpan>
{
	protected override FuzzrOf<TimeSpan> CreateFuzzr() => Fuzzr.TimeSpan();
	protected override FuzzrOf<TimeSpan> CreateRangedFuzzr(TimeSpan min, TimeSpan max) => Fuzzr.TimeSpan((int)min.Ticks, (int)max.Ticks);
	protected override (TimeSpan Min, TimeSpan Max) DefaultRange => (TimeSpan.MinValue, TimeSpan.MaxValue);
	protected override (TimeSpan Min, TimeSpan Max) ExampleRange => (new TimeSpan(5), new TimeSpan(7));
	protected override (TimeSpan Min, TimeSpan Max) MinimalRange => (new TimeSpan(0), new TimeSpan(1));
	protected override TimeSpan GetUpperBoundarySample(TimeSpan min, TimeSpan max) => max.Add(new TimeSpan(-1));
}