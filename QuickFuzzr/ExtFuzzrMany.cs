using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces a fixed number of values from the source fuzzr as an enumerable collection.
	/// Use for generating lists of consistent size, batch data, or when you need a specific count of generated items.
	/// </summary>
	public static FuzzrOf<IEnumerable<T>> Many<T>(this FuzzrOf<T> fuzzr, int number)
		=> state => new Result<IEnumerable<T>>(GetEnumerable(number, fuzzr, state), state);

	/// <summary>
	/// Creates a fuzzr that produces a variable number of values from the source fuzzr within the specified range.
	/// Use for generating lists of varying sizes to test collection handling, pagination, or dynamic data sets.
	/// </summary>
	public static FuzzrOf<IEnumerable<T>> Many<T>(this FuzzrOf<T> fuzzr, int min, int max)
		=> state => new Result<IEnumerable<T>>(GetEnumerable(Fuzzr.Int(min, max)(state).Value, fuzzr, state), state);

	private static List<T> GetEnumerable<T>(int number, FuzzrOf<T> fuzzr, State state)
	{
		List<T> values = [];
		var currentDepth = state.GetDepth(typeof(T));
		var (_, max) = state.GetDepthConstraint(typeof(T));
		if (currentDepth >= max)
			return values;
		state.Collecting.Push(true);
		for (int i = 0; i < number; i++)
		{
			values.Add(fuzzr(state).Value);
		}
		state.Collecting.Pop();
		return values;
	}
}