using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a generator that produces nullable values from the source generator, with 20% chance of returning null.
	/// Use for testing nullable value type handling, optional parameters, or scenarios where missing data needs to be simulated.
	/// </summary>
	public static FuzzrOf<T?> Nullable<T>(this FuzzrOf<T> generator)
		where T : struct
	{
		return Nullable(generator, 0.2);
	}

	/// <summary>
	/// Creates a generator that produces nullable values from the source generator with configurable null probability.
	/// Use when you need precise control over null frequency for testing specific edge cases or realistic data distributions.
	/// </summary>
	public static FuzzrOf<T?> Nullable<T>(this FuzzrOf<T> generator, double percentageOfNulls)
		where T : struct
	{
		return
			state =>
			{
				if (state.Random.NextDouble() < percentageOfNulls)
					return new Result<T?>(null, state);
				var val = generator(state).Value;
				return new Result<T?>(val, state);
			};
	}
}