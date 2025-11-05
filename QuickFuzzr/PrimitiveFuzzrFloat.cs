using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a generator that produces random single-precision floating-point values between 1 (inclusive) and 100 (exclusive).
	/// Use for generating efficient numeric data where full double precision is not required, such as graphics, physics, or performance-sensitive calculations.
	/// </summary>
	public static FuzzrOf<float> Float()
	{
		return Float(1, 100);
	}

	/// <summary>
	/// Creates a generator that produces random single-precision floating-point values within the specified range [min, max).
	/// Use when you need float values constrained to specific numeric bounds for 3D graphics, game development, or memory-optimized numeric testing.
	/// </summary>
	public static FuzzrOf<float> Float(float min, float max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<float>(((float)s.Random.NextDouble() * (max - min)) + min, s);
	}
}