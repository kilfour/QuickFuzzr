using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces a fixed number of values from the source fuzzr as an enumerable collection.
	/// Use for generating lists of consistent size, batch data, or when you need a specific count of generated items.
	/// </summary>
	public static FuzzrOf<IEnumerable<T>> Many<T>(this FuzzrOf<T> fuzzr, int number)
		=> state => new Result<IEnumerable<T>>(GetEnumerable(number, fuzzr, state), state);

	/// <summary>
	/// Creates a Fuzzr that produces a variable number of values from the source Fuzzr within the specified range (upper bound inclusive).
	/// Use for generating lists of varying sizes to test collection handling, pagination, or dynamic data sets.
	/// </summary>
	public static FuzzrOf<IEnumerable<T>> Many<T>(this FuzzrOf<T> fuzzr, int min, int max)
	{
		MinMax.Check(min, max);
		return state =>
		{
			var count = GetNumberOfItems(state, min, max);
			return new Result<IEnumerable<T>>(GetEnumerable(count, fuzzr, state), state);
		};
	}

	private static int GetNumberOfItems(State state, int min, int max)
	{
		if (max == int.MaxValue)
			return (int)state.Random.NextInt64(min, (long)int.MaxValue + 1);
		return state.Random.Next(min, max + 1);
	}

	private static List<T> GetEnumerable<T>(int number, FuzzrOf<T> fuzzr, State state)
	{
		var currentDepth = state.GetDepth(typeof(T));
		var (_, max) = state.GetDepthConstraint(typeof(T));
		if (currentDepth >= max)
			return [];
		List<T> values = new(number);
		try
		{
			state.Collecting.Push(true);
			for (int i = 0; i < number; i++)
			{
				values.Add(fuzzr(state).Value);
			}
		}
		finally { state.Collecting.Pop(); }
		return values;
	}
}
