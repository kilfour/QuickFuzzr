using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzzr
	{
		public static Generator<TimeSpan> TimeSpan()
		{
			return TimeSpan(1000);
		}

		public static Generator<TimeSpan> TimeSpan(int max)
		{
			return s => new Result<TimeSpan>(new TimeSpan(s.Random.Next(1, max)), s);
		}
	}
}