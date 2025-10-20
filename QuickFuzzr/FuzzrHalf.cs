using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<Half> Half()
	{
		return Half((Half)1, (Half)100);
	}

	public static Generator<Half> Half(Half min, Half max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<Half>(((Half)s.Random.NextDouble() * (max - min)) + min, s);
	}
}