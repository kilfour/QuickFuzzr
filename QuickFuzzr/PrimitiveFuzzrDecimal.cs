using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<decimal> Decimal()
	{
		return Decimal(1, 100);
	}

	public static FuzzrOf<decimal> Decimal(decimal min, decimal max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<decimal>(((decimal)s.Random.NextDouble() * (max - min)) + min, s);
	}
}