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
			var ticks = state.Random.NextInt64(min.Ticks, max.Ticks + 1);
			ticks = SnapToWholeSeconds(ticks);
			var value = new DateTime(ticks, DateTimeKind.Utc);
			return new Result<DateTime>(value, state);
		};

	private static long SnapToWholeSeconds(long ticks)
	{
		ticks -= ticks % System.TimeSpan.TicksPerSecond; // snap to whole seconds
		return ticks;
	}
}