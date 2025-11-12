using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random double-precision floating-point values between 1 (inclusive) and 100 (exclusive).
	/// Use for generating general-purpose numeric data where high precision is needed for scientific, engineering, or statistical testing.
	/// </summary>
	public static FuzzrOf<double> Double()
	{
		return Double(1, 100);
	}

	/// <summary>
	/// Creates a fuzzr that produces random double-precision floating-point values within the specified range [min, max).
	/// Use when you need double values constrained to specific numeric bounds for mathematical calculations or range-based validation.
	/// </summary>
	public static FuzzrOf<double> Double(double min, double max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<double>((s.Random.NextDouble() * (max - min)) + min, s);
	}
}