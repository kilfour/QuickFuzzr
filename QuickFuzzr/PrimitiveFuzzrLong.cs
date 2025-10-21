using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<long> Long()
	{
		return Long(1, 100);
	}

	public static FuzzrOf<long> Long(long min, long max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<long>((long)((s.Random.NextDouble() * (max - min)) + min), s);
	}
}