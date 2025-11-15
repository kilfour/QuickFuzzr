using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces random unsigned long integer values in the range [1, 100) (min inclusive, max exclusive).
	/// Use for generating large positive numeric data like file sizes, memory addresses, or any scenario requiring 64-bit non-negative whole numbers.
	/// </summary>
	public static FuzzrOf<ulong> ULong()
	{
		return ULong(1, 100);
	}

	/// <summary>
	/// Creates a Fuzzr that produces random unsigned long integer values in the range [min, max) (min inclusive, max exclusive).
	/// Use when you need large positive integer values constrained to specific bounds for big number arithmetic, unsigned ID generation, or high-range positive counters.
	/// </summary>
	public static FuzzrOf<ulong> ULong(ulong min, ulong max)
	{
		MinMax.Check(min, max);
		return s =>
		{
			ulong span = max - min;
			if (span == 0UL)
				return new Result<ulong>(min, s);
			ulong limit = ulong.MaxValue - (ulong.MaxValue % span);
			ulong r;
			var bytes = new byte[8];
			do
			{
				s.Random.NextBytes(bytes);
				r = BitConverter.ToUInt64(bytes, 0);
			}
			while (r >= limit);
			var value = min + (r % span);
			return new Result<ulong>(value, s);
		};
	}
}
