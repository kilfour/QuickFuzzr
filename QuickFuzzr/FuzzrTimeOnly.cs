using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<TimeOnly> TimeOnly()
		=> TimeOnly(System.TimeOnly.MinValue, System.TimeOnly.MaxValue);

	public static Generator<TimeOnly> TimeOnly(TimeOnly min, TimeOnly max)
	{
		if (max < min) throw new ArgumentOutOfRangeException(nameof(max), "max < min");

		var minTicks = min.Ticks;
		var maxTicks = max.Ticks;
		var span = maxTicks - minTicks;

		return s =>
		{
			var offset = s.Random.NextInt64(0, span + 1);
			var value = new TimeOnly(minTicks + offset);
			return new Result<TimeOnly>(value, s);
		};
	}
}
