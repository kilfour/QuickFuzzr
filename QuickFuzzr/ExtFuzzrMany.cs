using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a generator that produces a fixed number of values from the source generator as an enumerable collection.
	/// Use for generating lists of consistent size, batch data, or when you need a specific count of generated items.
	/// </summary>
	public static FuzzrOf<IEnumerable<T>> Many<T>(this FuzzrOf<T> generator, int number)
		=> state => new Result<IEnumerable<T>>(GetEnumerable(number, generator, state), state);

	/// <summary>
	/// Creates a generator that produces a variable number of values from the source generator within the specified range.
	/// Use for generating lists of varying sizes to test collection handling, pagination, or dynamic data sets.
	/// </summary>
	public static FuzzrOf<IEnumerable<T>> Many<T>(this FuzzrOf<T> generator, int min, int max)
		=> state => new Result<IEnumerable<T>>(GetEnumerable(Fuzzr.Int(min, max)(state).Value, generator, state), state);

	private static List<T> GetEnumerable<T>(int number, FuzzrOf<T> generator, State state)
	{
		List<T> values = [];
		var currentDepth = state.GetDepth(typeof(T));
		var (_, max) = state.GetDepthConstraint(typeof(T));
		if (currentDepth >= max)
			return values;
		state.Collecting.Push(true);
		for (int i = 0; i < number; i++)
		{
			values.Add(generator(state).Value);
		}
		state.Collecting.Pop();
		return values;
	}
}