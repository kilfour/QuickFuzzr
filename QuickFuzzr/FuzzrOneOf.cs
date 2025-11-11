using QuickFuzzr.UnderTheHood;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a generator that randomly selects one value from the provided options.
	/// Use for constrained choice scenarios like names, categories, or predefined enumerations.
	/// </summary>
	public static FuzzrOf<T> OneOf<T>(params T[] values)
	{
		return OneOf((IEnumerable<T>)values);
	}

	/// <summary>
	/// Creates a generator that randomly selects one value from the provided weighted options.
	/// Use when different options should appear with varying frequency, proportional to their weights.
	/// </summary>
	public static FuzzrOf<T> OneOf<T>(params (int Weight, T Value)[] values)
	{
		if (values is null || values.Length == 0)
			throw new OneOfEmptyOptionsException(typeof(T).Name);

		if (values.Any(w => w.Weight < 0))
			throw new NegativeWeightException(typeof(T).Name);

		long totalWeight = values.Aggregate(0L, (sum, v) => sum + v.Weight);
		if (totalWeight <= 0)
			throw new ZeroTotalWeightException(typeof(T).Name);

		return state =>
		{
			var roll = state.Random.NextInt64(1, totalWeight + 1);
			var cumulative = 0;
			foreach (var (weight, value) in values)
			{
				cumulative += weight;
				if (roll <= cumulative)
					return new Result<T>(value, state);
			}
			// Fallback (should never hit)
			return new Result<T>(values.Last().Value, state);
		};
	}

	/// <summary>
	/// Creates a generator that randomly selects one value from the provided collection.
	/// Use when your choice data comes from lists, arrays, or other enumerable sources.
	/// </summary>
	public static FuzzrOf<T> OneOf<T>(IEnumerable<T> values)
	{
		return
			s =>
			{
				if (!values.Any())
					throw new OneOfEmptyOptionsException(typeof(T).Name);
				var index = Int(0, values.Count())(s).Value;
				return new Result<T>(values.ElementAt(index), s);
			};
	}

	/// <summary>
	/// Creates a generator that randomly selects and executes one of the provided generators.
	/// Use for conditional generation strategies or when different value types require different generation logic.
	/// </summary>
	public static FuzzrOf<T> OneOf<T>(params FuzzrOf<T>[] values)
	{
		return
			s =>
			{
				if (values.Length == 0)
					throw new OneOfEmptyOptionsException(typeof(T).Name);
				var index = Int(0, values.Length)(s).Value;
				return new Result<T>(values.ElementAt(index)(s).Value, s);
			};
	}

	/// <summary>
	/// Creates a generator that randomly selects and executes one of the provided weighted generators.
	/// Use when different generation strategies should occur with varying frequency, proportional to their weights.
	/// </summary>
	public static FuzzrOf<T> OneOf<T>(params (int Weight, FuzzrOf<T> Generator)[] values)
	{
		if (values is null || values.Length == 0)
			throw new OneOfEmptyOptionsException(typeof(T).Name);

		if (values.Any(v => v.Weight <= 0))
			throw new ArgumentException("All weights must be positive.", nameof(values));

		long totalWeight = values.Aggregate(0L, (sum, v) => sum + v.Weight);
		return state =>
		{
			var roll = state.Random.NextInt64(1, totalWeight + 1);
			long cumulative = 0;
			foreach (var (weight, generator) in values)
			{
				cumulative += weight;
				if (roll <= cumulative)
					return generator(state);
			}
			// Fallback (should never occur)
			return values[^1].Generator(state);
		};
	}
}