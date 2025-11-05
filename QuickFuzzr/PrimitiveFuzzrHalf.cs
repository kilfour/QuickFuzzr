using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a generator that produces random Half-precision floating-point values between 1 (inclusive) and 100 (exclusive).
	/// Use for generating compact numeric data where full precision is not required, such as small measurements or percentages.
	/// </summary>
	public static FuzzrOf<Half> Half()
	{
		return Half((Half)1, (Half)100);
	}

	/// <summary>
	/// Creates a generator that produces random Half-precision floating-point values within the specified range [min, max).
	/// Use when you need constrained 16-bit floating-point values for memory-efficient numeric data generation.
	/// </summary>
	public static FuzzrOf<Half> Half(Half min, Half max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<Half>(((Half)s.Random.NextDouble() * (max - min)) + min, s);
	}
}