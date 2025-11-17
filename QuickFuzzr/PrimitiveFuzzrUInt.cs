using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces random unsigned integer values in the range [1, 100) (min inclusive, max exclusive).
	/// Use for generating positive numeric data like sizes, counts, or any scenario requiring non-negative whole numbers within a typical range.
	/// </summary>
	public static FuzzrOf<uint> UInt()
	{
		return UInt(1, 100);
	}

	/// <summary>
	/// Creates a Fuzzr that produces random unsigned integer values in the range [min, max) (min inclusive, max exclusive).
	/// Use when you need positive integer values constrained to specific bounds for array sizes, memory allocations, or unsigned numeric validation.
	/// </summary>
	public static FuzzrOf<uint> UInt(uint min, uint max)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThan(min, max);
		return s =>
		{
			var span = (ulong)(max - min);
			if (span == 0)
				return new Result<uint>(min, s);
			var offset = (ulong)s.Random.NextInt64(0, (long)span);
			return new Result<uint>(min + (uint)offset, s);
		};
	}
}
