using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<decimal> Decimal() => Decimal(1, 100);
	public static FuzzrOf<decimal> Decimal(decimal min, decimal max)
		=> MinMax.Check(min, max, s => new Result<decimal>(((decimal)s.Random.NextDouble() * (max - min)) + min, s));
}