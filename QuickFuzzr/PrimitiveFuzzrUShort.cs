using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random unsigned short integer values between 1 (inclusive) and 100 (exclusive).
	/// Use for generating compact positive numeric data like port numbers, small counters, or any scenario requiring 16-bit non-negative whole numbers.
	/// </summary>
	public static FuzzrOf<ushort> UShort() => UShort(1, 100);

	/// <summary>
	/// Creates a fuzzr that produces random unsigned short integer values within the specified range [min, max).
	/// Use when you need small positive integer values constrained to specific bounds for network ports, array indices, or unsigned 16-bit validation.
	/// </summary>
	public static FuzzrOf<ushort> UShort(ushort min, ushort max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
		return s => new Result<ushort>((ushort)s.Random.Next(min, max), s);
	}
}