using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{

	/// <summary>
	/// Creates a fuzzr that produces nullable reference values from the source fuzzr, with 20% chance of returning null.
	/// Use for testing null reference handling, optional object parameters, or scenarios where reference types might be missing.
	/// </summary>
	public static FuzzrOf<T?> NullableRef<T>(this FuzzrOf<T> fuzzr)
		where T : class
	{
		return NullableRef(fuzzr, 0.2);
	}

	/// <summary>
	/// Creates a fuzzr that produces nullable reference values from the source fuzzr with configurable null probability.
	/// Use when you need controlled null injection for reference types to test null-checking logic or optional dependencies.
	/// </summary>
	public static FuzzrOf<T?> NullableRef<T>(this FuzzrOf<T> fuzzr, double percentageOfNulls)
		where T : class
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