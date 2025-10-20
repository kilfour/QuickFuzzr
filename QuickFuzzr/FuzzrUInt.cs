using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<uint> UInt()
	{
		return UInt(1, 100);
	}

	public static Generator<uint> UInt(uint min, uint max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
		return s => new Result<uint>((uint)s.Random.Next((int)min, (int)max), s);
	}
}