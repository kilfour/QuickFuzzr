using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a generator that produces random unsigned integer values between 1 (inclusive) and 100 (exclusive).
	/// Use for generating positive numeric data like sizes, counts, or any scenario requiring non-negative whole numbers within a typical range.
	/// </summary>
	public static FuzzrOf<uint> UInt()
	{
		return UInt(1, 100);
	}

	/// <summary>
	/// Creates a generator that produces random unsigned integer values within the specified range [min, max).
	/// Use when you need positive integer values constrained to specific bounds for array sizes, memory allocations, or unsigned numeric validation.
	/// </summary>
	public static FuzzrOf<uint> UInt(uint min, uint max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
		return s => new Result<uint>((uint)s.Random.Next((int)min, (int)max), s);
	}
}