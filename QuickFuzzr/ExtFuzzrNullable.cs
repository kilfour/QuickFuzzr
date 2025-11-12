using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces nullable values from the source fuzzr, with 20% chance of returning null.
	/// Use for testing nullable value type handling, optional parameters, or scenarios where missing data needs to be simulated.
	/// </summary>
	public static FuzzrOf<T?> Nullable<T>(this FuzzrOf<T> fuzzr)
		where T : struct
	{
		return Nullable(fuzzr, 0.2);
	}

	/// <summary>
	/// Creates a fuzzr that produces nullable values from the source fuzzr with configurable null probability.
	/// Use when you need precise control over null frequency for testing specific edge cases or realistic data distributions.
	/// </summary>
	public static FuzzrOf<T?> Nullable<T>(this FuzzrOf<T> fuzzr, double percentageOfNulls)
		where T : struct
	{
		return
			state =>
			{
				if (state.Random.NextDouble() < percentageOfNulls)
					return new Result<T?>(null, state);
				var val = fuzzr(state).Value;
				return new Result<T?>(val, state);
			};
	}
}