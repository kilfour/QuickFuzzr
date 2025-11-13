using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random TimeOnly values across the full possible time range (00:00:00 to 23:59:59.9999999 inclusive).
	/// Use for generating time values to test time-based logic, scheduling systems, or any scenario requiring complete temporal coverage.
	/// </summary>
	public static FuzzrOf<TimeOnly> TimeOnly()
		=> TimeOnly(System.TimeOnly.MinValue, System.TimeOnly.MaxValue);

	/// <summary>
	/// Creates a fuzzr that produces random TimeOnly values within the specified inclusive time range.
	/// Use when you need time values constrained to specific periods like business hours, shift times, or time-bound operations.
	/// </summary>
	public static FuzzrOf<TimeOnly> TimeOnly(TimeOnly min, TimeOnly max)
		=> MinMax.Check(min, max,
			state =>
			{
				var minTicks = min.Ticks;
				var maxTicks = max.Ticks;
				var span = maxTicks - minTicks;
				var offset = state.Random.NextInt64(0, span + 1);
				var value = new TimeOnly(minTicks + offset);
				return new Result<TimeOnly>(value, state);
			});
}
