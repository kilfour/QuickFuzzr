using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	public static Generator<IEnumerable<T>> Many<T>(this Generator<T> generator, int number)
		=> state => new Result<IEnumerable<T>>([.. GetEnumerable(number, generator, state)], state);

	public static Generator<IEnumerable<T>> Many<T>(this Generator<T> generator, int min, int max)
		=> state => new Result<IEnumerable<T>>([.. GetEnumerable(Fuzzr.Int(min, max)(state).Value, generator, state)], state);

	private static IEnumerable<T> GetEnumerable<T>(int number, Generator<T> generator, State s)
	{
		for (int i = 0; i < number; i++) { yield return generator(s).Value; }
	}
}