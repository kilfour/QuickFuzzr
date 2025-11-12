using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random decimal values between 1 (inclusive) and 100 (exclusive) with 2 decimal places.
	/// Use for generating precise numeric data suitable for financial calculations, currency values, or exact decimal arithmetic.
	/// </summary>
	public static FuzzrOf<decimal> Decimal() => Decimal(1, 100, 2);

	/// <summary>
	/// Creates a fuzzr that produces random decimal values between 1 (inclusive) and 100 (exclusive) with specified precision.
	/// Use for generating decimal values with controlled decimal places for currency formatting, fixed-point calculations, or display requirements.
	/// </summary>
	public static FuzzrOf<decimal> Decimal(int precision) => Decimal(1, 100, precision);

	/// <summary>
	/// Creates a fuzzr that produces random decimal values within the specified range [min, max).
	/// Use when you need high-precision decimal values constrained to specific numeric bounds for financial or scientific testing.
	/// </summary>
	public static FuzzrOf<decimal> Decimal(decimal min, decimal max)
		=> MinMax.Check(min, max, s => new Result<decimal>(((decimal)s.Random.NextDouble() * (max - min)) + min, s));

	/// <summary>
	/// Creates a fuzzr that produces random decimal values within the specified range [min, max) with controlled precision.
	/// Use when you need precise decimal values with fixed decimal places for financial reporting, measurement data, or formatted numeric output.
	/// </summary>
	public static FuzzrOf<decimal> Decimal(decimal min, decimal max, int precision)
		=> Decimal(min, max).Apply(d => Math.Round(d, precision));
}