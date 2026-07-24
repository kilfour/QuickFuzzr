using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
    /// <summary>
    /// Creates a Fuzzr that produces random boolean values with equal probability of true or false.
    /// Use for generating flags, conditions, or any binary decision points in your test data.
	/// </summary>
	public static FuzzrOf<bool> Bool() => PrimitiveDefault(BuiltInBool());

	private static FuzzrOf<bool> BuiltInBool() =>
		state => new Result<bool>(state.Random.Next(0, 2) > 0, state);
}
