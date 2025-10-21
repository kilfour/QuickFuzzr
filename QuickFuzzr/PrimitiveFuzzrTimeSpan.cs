using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<TimeSpan> TimeSpan()
	{
		return TimeSpan(1000);
	}

	public static FuzzrOf<TimeSpan> TimeSpan(int max)
	{
		return s => new Result<TimeSpan>(new TimeSpan(s.Random.Next(1, max)), s);
	}
}