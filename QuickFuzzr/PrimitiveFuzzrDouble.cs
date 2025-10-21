using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<double> Double()
	{
		return Double(1, 100);
	}

	public static FuzzrOf<double> Double(double min, double max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<double>((s.Random.NextDouble() * (max - min)) + min, s);
	}
}