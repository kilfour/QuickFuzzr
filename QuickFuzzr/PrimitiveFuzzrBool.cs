using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random boolean values with equal probability of true or false.
	/// Use for generating flags, conditions, or any binary decision points in your test data.
	/// </summary>
	public static FuzzrOf<bool> Bool()
	{
		return s => new Result<bool>(s.Random.Next(0, 2) > 0, s);
	}
}