using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	public static FuzzrOf<IEnumerable<T>> Many<T>(this FuzzrOf<T> generator, int number)
		=> state => new Result<IEnumerable<T>>(GetEnumerable(number, generator, state), state);

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