using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<float> Float()
	{
		return Float(1, 100);
	}

	public static FuzzrOf<float> Float(float min, float max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s => new Result<float>(((float)s.Random.NextDouble() * (max - min)) + min, s);
	}
}