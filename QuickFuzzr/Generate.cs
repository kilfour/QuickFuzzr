using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static class GenerateIt
{
	/// <summary>
	/// Executes the generator and returns the produced value, optionally using a specific seed for reproducible results.
	/// Use to materialize generated data for tests, samples, or when you need concrete instances from your Fuzzr definitions.
	/// </summary>
	public static T Generate<T>(this FuzzrOf<T> generator, int? seed = null) => generator(new State(seed)).Value;
}