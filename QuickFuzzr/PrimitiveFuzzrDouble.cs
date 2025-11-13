using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random double-precision floating-point values in the range [1, 100) (min inclusive, max exclusive).
	/// Use for generating general-purpose numeric data where high precision is needed for scientific, engineering, or statistical testing.
	/// </summary>
	public static FuzzrOf<double> Double()
	{
		return Double(1, 100);
	}

	/// <summary>
	/// Creates a fuzzr that produces random double-precision floating-point values in the range [min, max) (min inclusive, max exclusive).
	/// Use when you need double values constrained to specific numeric bounds for mathematical calculations or range-based validation.
	/// </summary>
	public static FuzzrOf<double> Double(double min, double max)
	{
		MinMax.Check(min, max);
		return s => new Result<double>((s.Random.NextDouble() * (max - min)) + min, s);
	}
}
