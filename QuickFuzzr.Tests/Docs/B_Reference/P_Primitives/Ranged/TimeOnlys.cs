using QuickPulse.Explains;
using WibblyWobbly;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Ranged;

[DocFile]
[DocFileHeader("TimeOnlys")]
[DocContent("Use `Fuzzr.TimeOnly()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces random times between midnight and 23:59:59.")]
[DocContent("- **Default:** min = 00:00:00, max = 23:59:59.9999999).")]
public class TimeOnlys : RangedPrimitive<TimeOnly>
{
	protected override FuzzrOf<TimeOnly> CreateFuzzr()
		=> Fuzzr.TimeOnly();

	protected override FuzzrOf<TimeOnly> CreateRangedFuzzr(TimeOnly min, TimeOnly max)
		=> Fuzzr.TimeOnly(min, max);

	protected override (TimeOnly Min, TimeOnly Max) DefaultRange
		=> From(TimeOnly.MinValue)
			.To(TimeOnly.MaxValue);

	protected override (TimeOnly Min, TimeOnly Max) ExampleRange
		=> From(6.OClock())
			.To(5.Past(6));

	protected override (TimeOnly Min, TimeOnly Max) MinimalRange
		=> (new TimeOnly(0), new TimeOnly(1));

	protected override bool UpperBoundExclusive => false;
}