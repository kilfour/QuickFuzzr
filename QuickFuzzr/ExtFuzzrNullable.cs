using QuickFuzzr.UnderTheHood;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

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

	/// <summary>
	/// Creates a generator that produces nullable reference values from the source generator, with 20% chance of returning null.
	/// Use for testing null reference handling, optional object parameters, or scenarios where reference types might be missing.
	/// </summary>
	public static FuzzrOf<T?> NullableRef<T>(this FuzzrOf<T> generator)
		where T : class
	{
		return NullableRef(generator, 0.2);
	}

	/// <summary>
	/// Creates a generator that produces nullable reference values from the source generator with configurable null probability.
	/// Use when you need controlled null injection for reference types to test null-checking logic or optional dependencies.
	/// </summary>
	public static FuzzrOf<T?> NullableRef<T>(this FuzzrOf<T> generator, double percentageOfNulls)
		where T : class
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

	/// <summary>
	/// Creates a generator that filters out null values from a nullable generator, ensuring only non-null values are produced.
	/// Use when you need guaranteed non-null values for testing code paths that require valid data or cannot handle null inputs.
	/// </summary>
	public static FuzzrOf<T?> NeverReturnNull<T>(this FuzzrOf<T?> generator) where T : struct
	{
		return s =>
		{
			var limit = s.RetryLimit;
			for (var i = 0; i < limit; i++)
			{
				var v = generator(s).Value;
				if (v is not null)
					return new Result<T?>(v, s);
			}
			throw new NonNullValueExhaustedException(typeof(T).Name, limit);
		};
	}

	// public static FuzzrOf<T?> NeverReturnNullRef<T>(this FuzzrOf<T?> generator) where T : class
	// {
	// 	return s =>
	// 	{
	// 		var limit = s.RetryLimit;
	// 		for (var i = 0; i < limit; i++)
	// 		{
	// 			var v = generator(s).Value;
	// 			if (v is not null)
	// 				return new Result<T?>(v, s);
	// 		}
	// 		throw new NonNullValueExhaustedException(typeof(T).Name, limit);
	// 	};
	// }
}