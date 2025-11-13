using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random TimeSpan values in the range [1, 1000) ticks (min inclusive, max exclusive).
	/// Use for generating short duration values to test timing logic, intervals, or any scenario requiring basic time span data.
	/// </summary>
	public static FuzzrOf<TimeSpan> TimeSpan()
	{
		return TimeSpan(1000);
	}

	/// <summary>
	/// Creates a fuzzr that produces random TimeSpan values in the range [1, max) ticks (min inclusive, max exclusive).
	/// Use when you need duration values constrained to specific ranges for timeout testing, performance measurements, or interval validation.
	/// </summary>
	public static FuzzrOf<TimeSpan> TimeSpan(int max)
	{
		return s => new Result<TimeSpan>(new TimeSpan(s.Random.Next(1, max)), s);
	}
}
