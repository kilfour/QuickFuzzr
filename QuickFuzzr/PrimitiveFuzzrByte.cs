using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<byte> Byte()
		=> Byte(byte.MinValue, byte.MaxValue);

	public static FuzzrOf<byte> Byte(int min, int max)
	{
		if (min < byte.MinValue)
			throw new ArgumentOutOfRangeException(nameof(min), $"Must be ≥ {byte.MinValue}.");

		if (max > byte.MaxValue)
			throw new ArgumentOutOfRangeException(nameof(max), $"Must be ≤ {byte.MaxValue}.");

		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

		return s =>
		{
			var value = (byte)s.Random.Next(min, max + 1);
			return new Result<byte>(value, s);
		};
	}
}
