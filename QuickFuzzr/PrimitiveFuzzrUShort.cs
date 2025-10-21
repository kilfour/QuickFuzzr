using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<ushort> UShort() => UShort(1, 100);
	public static FuzzrOf<ushort> UShort(ushort min, ushort max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
		return s => new Result<ushort>((ushort)s.Random.Next(min, max), s);
	}
}