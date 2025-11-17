using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces random single-precision floating-point values in the range [1, 100) (min inclusive, max exclusive).
	/// Use for generating efficient numeric data where full double precision is not required, such as graphics, physics, or performance-sensitive calculations.
	/// </summary>
	public static FuzzrOf<float> Float()
	{
		return Float(1, 100);
	}

	/// <summary>
	/// Creates a Fuzzr that produces random single-precision floating-point values in the range [min, max) (min inclusive, max exclusive).
	/// Use when you need float values constrained to specific numeric bounds for 3D graphics, game development, or memory-optimized numeric testing.
	/// </summary>
	public static FuzzrOf<float> Float(float min, float max)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThan(min, max);
		return s => new Result<float>(((float)s.Random.NextDouble() * (max - min)) + min, s);
	}
}
