using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a generator that produces random long integer values between 1 (inclusive) and 100 (exclusive).
	/// Use for generating large numeric identifiers, file sizes, or any scenario requiring 64-bit whole numbers within a typical range.
	/// </summary>
	public static FuzzrOf<long> Long()
	{
		return Long(1, 100);
	}

	/// <summary>
	/// Creates a generator that produces random long integer values within the specified range [min, max).
	/// Use when you need large integer values constrained to specific bounds for testing big number arithmetic, database IDs, or high-range counters.
	/// </summary>
	public static FuzzrOf<long> Long(long min, long max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<long>((long)((s.Random.NextDouble() * (max - min)) + min), s);
	}
}