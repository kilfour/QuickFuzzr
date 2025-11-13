using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random DateTime values between January 1, 1970 and December 31, 2020 (inclusive), snapped to whole seconds.
	/// Use for generating realistic timestamp values within a common historical range for testing temporal logic and events.
	/// </summary>
	public static FuzzrOf<DateTime> DateTime()
	{
		return DateTime(new DateTime(1970, 1, 1), new DateTime(2020, 12, 31));
	}

	/// <summary>
	/// Creates a fuzzr that produces random DateTime values within the specified inclusive range, snapped to whole seconds.
	/// Use when you need timestamp values constrained to specific time periods with simplified second-level precision.
	/// </summary>
	public static FuzzrOf<DateTime> DateTime(DateTime min, DateTime max)
		=> MinMax.Check(min, max, DateTimeInternal(min, max));

	private static FuzzrOf<DateTime> DateTimeInternal(DateTime min, DateTime max) =>
		state =>
		{
			const long tps = System.TimeSpan.TicksPerSecond;
			long minTicks = min.Ticks;
			long maxTicks = max.Ticks;
			// Compute first whole-second start >= min
			long minSecStart = (minTicks % tps == 0) ? minTicks : (minTicks + (tps - (minTicks % tps)));
			// Compute last whole-second start <= max (inclusive upper bound)
			long maxSecStart = (maxTicks / tps) * tps;
			if (minSecStart > maxSecStart)
			{
				// No whole-second boundary inside [min, max]; sample uniformly at tick resolution (inclusive)
				var ticksInclusive = state.Random.NextInt64(minTicks, maxTicks + 1);
				var valueInclusive = new DateTime(ticksInclusive, DateTimeKind.Utc);
				return new Result<DateTime>(valueInclusive, state);
			}
			long secondsCount = ((maxSecStart - minSecStart) / tps) + 1;
			long secondOffset = state.Random.NextInt64(0, secondsCount);
			var ticks = minSecStart + (secondOffset * tps);
			var value = new DateTime(ticks, DateTimeKind.Utc);
			return new Result<DateTime>(value, state);
		};
}
