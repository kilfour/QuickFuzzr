using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<ulong> ULong()
	{
		return ULong(1, 100);
	}

	public static FuzzrOf<ulong> ULong(ulong min, ulong max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<ulong>((ulong)((s.Random.NextDouble() * (max - min)) + min), s);
	}
}