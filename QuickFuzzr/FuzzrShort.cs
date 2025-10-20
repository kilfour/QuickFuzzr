using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<short> Short()
	{
		return Short(1, 100);
	}

	public static Generator<short> Short(short min, short max)
	{
		return s => new Result<short>((short)((s.Random.NextDouble() * (max - min)) + min), s);
	}
}