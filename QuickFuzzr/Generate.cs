using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static class GenerateIt
{
	public static T Generate<T>(this FuzzrOf<T> generator) => generator(new State()).Value;
}