using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static class GenerateIt
{
	/// <summary>
	/// Executes the fuzzr and returns the produced value, optionally using a specific seed for reproducible results.
	/// Use to materialize generated data for tests, samples, or when you need concrete instances from your Fuzzr definitions.
	/// </summary>
	public static T Generate<T>(this FuzzrOf<T> fuzzr, int? seed = null) => fuzzr(new State(seed)).Value;
}