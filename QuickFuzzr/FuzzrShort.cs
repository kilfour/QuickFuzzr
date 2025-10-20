using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<short> Short() => Short(1, 100);
	public static Generator<short> Short(short min, short max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
		return state => new Result<short>((short)((state.Random.NextDouble() * (max - min)) + min), state);
	}
}