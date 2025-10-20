using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static class GenerateIt
{
	public static T Generate<T>(this Generator<T> generator)
	{
		return generator(new State()).Value;
	}
}