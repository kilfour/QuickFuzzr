using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces random byte values across the full inclusive range [0, 255].
	/// Use for generating small numeric values, flags, or when working with binary data formats.
	/// </summary>
	public static FuzzrOf<byte> Byte()
		=> Byte(byte.MinValue, byte.MaxValue);

	/// <summary>
	/// Creates a Fuzzr that produces random byte values within the inclusive range [min, max].
	/// Use when you need constrained byte values for specific scenarios like ports, small counters, or bounded numeric fields.
	/// </summary>
	public static FuzzrOf<byte> Byte(byte min, byte max)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThan(min, max);
		return state =>
		{
			var value = (byte)state.Random.Next(min, max + 1);
			return new Result<byte>(value, state);
		};
	}
}
