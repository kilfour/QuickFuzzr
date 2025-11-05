using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a generator that produces random decimal values between 1 (inclusive) and 100 (exclusive).
	/// Use for generating precise numeric data suitable for financial calculations, currency values, or exact decimal arithmetic.
	/// </summary>
	public static FuzzrOf<decimal> Decimal() => Decimal(1, 100);

	/// <summary>
	/// Creates a generator that produces random decimal values within the specified range [min, max).
	/// Use when you need high-precision decimal values constrained to specific numeric bounds for financial or scientific testing.
	/// </summary>
	public static FuzzrOf<decimal> Decimal(decimal min, decimal max)
		=> MinMax.Check(min, max, s => new Result<decimal>(((decimal)s.Random.NextDouble() * (max - min)) + min, s));
}