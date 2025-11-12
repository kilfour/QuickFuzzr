using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random integer values between 1 (inclusive) and 100 (exclusive).
	/// Use for generating common numeric test data like counts, ages, or any scenario requiring whole numbers within a typical range.
	/// </summary>
	public static FuzzrOf<int> Int()
	{
		return Int(1, 100);
	}

	/// <summary>
	/// Creates a fuzzr that produces random integer values within the specified range [min, max).
	/// Use when you need integer values constrained to specific bounds for boundary testing, loop iterations, or range validation.
	/// </summary>
	public static FuzzrOf<int> Int(int min, int max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
		return s => new Result<int>(s.Random.Next(min, max), s);
	}
}