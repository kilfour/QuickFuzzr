using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<DateTime> DateTime()
	{
		return DateTime(new DateTime(1970, 1, 1), new DateTime(2020, 12, 31));
	}

	public static Generator<DateTime> DateTime(DateTime min, DateTime max)
	{
		return
			s =>
				{
					var ticks = s.Random.NextInt64(min.Ticks, max.Ticks + 1);
					ticks -= ticks % System.TimeSpan.TicksPerSecond; // snap to whole seconds
					var value = new DateTime(ticks, DateTimeKind.Utc);
					return new Result<DateTime>(value, s);
				};
	}
}