using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<IEnumerable<T>> Many<T>(this Generator<T> generator, int number)
	{
		return s => new Result<IEnumerable<T>>(GetEnumerable(number, generator, s).ToArray(), s);
	}

	public static Generator<IEnumerable<T>> Many<T>(this Generator<T> generator, int min, int max)
	{
		return s => new Result<IEnumerable<T>>(GetEnumerable(Int(min, max)(s).Value, generator, s).ToArray(), s);
	}

	private static IEnumerable<T> GetEnumerable<T>(int number, Generator<T> generator, State s)
	{
		for (int i = 0; i < number; i++)
		{
			yield return generator(s).Value;
		}
	}
}