using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<DateTime> DateTime()
	{
		return DateTime(new DateTime(1970, 1, 1), new DateTime(2020, 12, 31));
	}

	public static FuzzrOf<DateTime> DateTime(DateTime min, DateTime max)
		=> MinMax.Check(min, max, DateTimeInternal(min, max));

	public static FuzzrOf<DateTime> DateTimeInternal(DateTime min, DateTime max) =>
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