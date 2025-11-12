using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random byte values across the full range (0-255).
	/// Use for generating small numeric values, flags, or when working with binary data formats.
	/// </summary>
	public static FuzzrOf<byte> Byte()
		=> Byte(byte.MinValue, byte.MaxValue);

	/// <summary>
	/// Creates a fuzzr that produces random byte values within the specified inclusive range.
	/// Use when you need constrained byte values for specific scenarios like ports, small counters, or bounded numeric fields.
	/// </summary>
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
