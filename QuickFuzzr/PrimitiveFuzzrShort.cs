using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<short> Short() => Short(1, 100);
	public static FuzzrOf<short> Short(short min, short max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
		return state => new Result<short>((short)((state.Random.NextDouble() * (max - min)) + min), state);
	}
}