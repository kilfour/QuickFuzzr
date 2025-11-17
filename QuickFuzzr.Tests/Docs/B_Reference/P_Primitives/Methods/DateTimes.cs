using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;
using WibblyWobbly;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.Methods;

[DocFile]
[DocFileHeader("DateTimes")]
[DocContent("Use `Fuzzr.DateTime()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Produces `DateTime` values between 1970-01-01 and 2020-12-31 (inclusive), snapped to whole seconds.")]
[DocContent("- **Default:** min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)) inclusive, snapped to whole seconds.")]
[DocContent("- The overload `Fuzzr.DateTime(DateTime min, DateTime max)` generates a `DateTime` in the inclusive range [min, max], snapped to whole seconds.")]
public class DateTimes : RangedPrimitive<DateTime>
{
	protected override FuzzrOf<DateTime> CreateFuzzr()
		=> Fuzzr.DateTime();

	protected override FuzzrOf<DateTime> CreateRangedFuzzr(DateTime min, DateTime max)
		=> Fuzzr.DateTime(min, max);

	protected override (DateTime Min, DateTime Max) DefaultRange
		=> From(1.January(1970).At(1.OClock()))
			.To(31.December(2020).At(1.OClock()));

	protected override (DateTime Min, DateTime Max) ExampleRange
		=> From(1.January(2000).At(1.OClock()))
			.To(5.January(2000).At(1.OClock()));

	protected override (DateTime Min, DateTime Max) MinimalRange
		=> From(1.January(2000).At(1.OClock().Seconds(0)))
			.To(1.January(2000).At(1.OClock().Seconds(1)));

	protected override bool UpperBoundExclusive => false;
}
