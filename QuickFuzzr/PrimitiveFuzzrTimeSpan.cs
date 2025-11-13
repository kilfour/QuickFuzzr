using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random TimeSpan values between 1 and 1000 (exclusive) ticks .
	/// Use for generating short duration values to test timing logic, intervals, or any scenario requiring basic time span data.
	/// </summary>
	public static FuzzrOf<TimeSpan> TimeSpan()
	{
		return TimeSpan(1000);
	}

	/// <summary>
	/// Creates a fuzzr that produces random TimeSpan values between 1 and the specified maximum number of ticks (exclusive).
	/// Use when you need duration values constrained to specific ranges for timeout testing, performance measurements, or interval validation.
	/// </summary>
	public static FuzzrOf<TimeSpan> TimeSpan(int max)
	{
		return s => new Result<TimeSpan>(new TimeSpan(s.Random.Next(1, max)), s);
	}
}