using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random integer values in the range [1, 100) (min inclusive, max exclusive).
	/// Use for generating common numeric test data like counts, ages, or any scenario requiring whole numbers within a typical range.
	/// </summary>
	public static FuzzrOf<int> Int()
	{
		return Int(1, 100);
	}

	/// <summary>
	/// Creates a fuzzr that produces random integer values in the range [min, max) (min inclusive, max exclusive).
	/// Use when you need integer values constrained to specific bounds for boundary testing, loop iterations, or range validation.
	/// </summary>
	public static FuzzrOf<int> Int(int min, int max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
		return s => new Result<int>(s.Random.Next(min, max), s);
	}
}
