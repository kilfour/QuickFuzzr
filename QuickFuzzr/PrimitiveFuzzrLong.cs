using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random long integer values in the range [1, 100) (min inclusive, max exclusive).
	/// Use for generating large numeric identifiers, file sizes, or any scenario requiring 64-bit whole numbers within a typical range.
	/// </summary>
	public static FuzzrOf<long> Long()
	{
		return Long(1, 100);
	}

	/// <summary>
	/// Creates a fuzzr that produces random long integer values in the range [min, max) (min inclusive, max exclusive).
	/// Use when you need large integer values constrained to specific bounds for testing big number arithmetic, database IDs, or high-range counters.
	/// </summary>
	public static FuzzrOf<long> Long(long min, long max)
	{
		MinMax.Check(min, max);
		return s =>
		{
			if (min == max)
				return new Result<long>(min, s);
			return new Result<long>(s.Random.NextInt64(min, max), s);
		};
	}
}
