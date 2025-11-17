using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces random short integer values in the range [1, 100) (min inclusive, max exclusive).
	/// Use for generating compact numeric data like small counters, port numbers, or any scenario requiring 16-bit whole numbers within a typical range.
	/// </summary>
	public static FuzzrOf<short> Short() => Short(1, 100);

	/// <summary>
	/// Creates a Fuzzr that produces random short integer values in the range [min, max) (min inclusive, max exclusive).
	/// Use when you need small integer values constrained to specific bounds for memory-efficient numeric data or legacy system interfaces.
	/// </summary>
	public static FuzzrOf<short> Short(short min, short max)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThan(min, max);
		return state => new Result<short>((short)state.Random.Next(min, max), state);
	}
}
