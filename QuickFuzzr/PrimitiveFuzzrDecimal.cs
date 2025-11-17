using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces random decimal values in the range [1, 100)
	/// (1 inclusive, 100 exclusive) with 2 decimal places.
	/// Use for generating precise numeric data suitable for financial calculations,
	/// currency values, or exact decimal arithmetic.
	/// </summary>
	public static FuzzrOf<decimal> Decimal() => Decimal(1, 100, 2);

	/// <summary>
	/// Creates a Fuzzr that produces random decimal values in the range [1, 100)
	/// (min inclusive, max exclusive) with up to the specified precision.
	/// Use for generating decimal values with controlled decimal places for currency
	/// formatting, fixed-point calculations, or display requirements.
	/// </summary>
	public static FuzzrOf<decimal> Decimal(int precision) => Decimal(1, 100, precision);

	/// <summary>
	/// Creates a Fuzzr that produces random decimal values in the range [min, max)
	/// (min inclusive, max exclusive).
	/// Use when you need high-precision decimal values constrained to specific
	/// numeric bounds for financial or scientific testing.
	/// </summary>
	public static FuzzrOf<decimal> Decimal(decimal min, decimal max)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThan(min, max);
		return state => new Result<decimal>(((decimal)state.Random.NextDouble() * (max - min)) + min, state);
	}

	/// <summary>
	/// Creates a Fuzzr that produces random decimal values in the range [min, max)
	/// (min inclusive, max exclusive) with up to the specified precision.
	/// Use when you need precise decimal values with fixed decimal places for
	/// financial reporting, measurement data, or formatted numeric output.
	/// </summary>
	public static FuzzrOf<decimal> Decimal(decimal min, decimal max, int precision)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(precision, 0);
		return Decimal(min, max).Apply(d =>
			{
				decimal scale = 1m;
				for (int i = 0; i < precision; i++) scale *= 10m;
				var truncated = Math.Floor((d - min) * scale) / scale + min;
				return Math.Round(truncated, precision);
			});
	}
}
