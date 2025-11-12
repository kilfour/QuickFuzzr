using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random unsigned long integer values between 1 (inclusive) and 100 (exclusive).
	/// Use for generating large positive numeric data like file sizes, memory addresses, or any scenario requiring 64-bit non-negative whole numbers.
	/// </summary>
	public static FuzzrOf<ulong> ULong()
	{
		return ULong(1, 100);
	}

	/// <summary>
	/// Creates a fuzzr that produces random unsigned long integer values within the specified range [min, max).
	/// Use when you need large positive integer values constrained to specific bounds for big number arithmetic, unsigned ID generation, or high-range positive counters.
	/// </summary>
	public static FuzzrOf<ulong> ULong(ulong min, ulong max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<ulong>((ulong)((s.Random.NextDouble() * (max - min)) + min), s);
	}
}