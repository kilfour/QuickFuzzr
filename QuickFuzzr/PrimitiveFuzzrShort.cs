using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random short integer values between 1 (inclusive) and 100 (exclusive).
	/// Use for generating compact numeric data like small counters, port numbers, or any scenario requiring 16-bit whole numbers within a typical range.
	/// </summary>
	public static FuzzrOf<short> Short() => Short(1, 100);

	/// <summary>
	/// Creates a fuzzr that produces random short integer values within the specified range [min, max).
	/// Use when you need small integer values constrained to specific bounds for memory-efficient numeric data or legacy system interfaces.
	/// </summary>
	public static FuzzrOf<short> Short(short min, short max)
	{
		if (min > max)
			throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
		return state => new Result<short>((short)((state.Random.NextDouble() * (max - min)) + min), state);
	}
}