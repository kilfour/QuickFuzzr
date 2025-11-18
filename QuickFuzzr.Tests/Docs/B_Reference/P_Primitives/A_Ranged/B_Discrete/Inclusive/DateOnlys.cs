using QuickPulse.Explains;
using WibblyWobbly;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.A_Ranged.B_Discrete.Inclusive;

[DocFileHeader("DateOnlys")]
[DocContent("Use `Fuzzr.DateOnly()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Creates `DateOnly` values between 1970-01-01 and 2020-12-31 (by default).")]
[DocContent("- **Default Range:** min = 'DateOnly(1970, 1, 1)', max = 'DateOnly(2020, 12, 31)'.")]
public class DateOnlys : RangedPrimitive<DateOnly>
{
	protected override FuzzrOf<DateOnly> CreateFuzzr()
		=> Fuzzr.DateOnly();

	protected override FuzzrOf<DateOnly> CreateRangedFuzzr(DateOnly min, DateOnly max)
		=> Fuzzr.DateOnly(min, max);

	protected override (DateOnly Min, DateOnly Max) DefaultRange
		=> From(1.January(1970))
			.To(31.December(2020));

	protected override (DateOnly Min, DateOnly Max) ExampleRange
		=> From(1.January(2000))
			.To(5.January(2000));

	protected override (DateOnly Min, DateOnly Max) MinimalRange
		=> From(1.January(2000))
		  .To(2.January(2000));

	protected override bool UpperBoundExclusive => false;
}