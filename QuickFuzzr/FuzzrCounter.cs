using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces incrementing counter values, starting from 1 and increasing with each generation.
	/// Use for generating unique sequential identifiers, test case numbers, or any scenario requiring monotonically increasing values.
	/// </summary>
	public static FuzzrOf<int> Counter(object key)
	{
		return state =>
		{
			var cnt = state.Get(key, 0) + 1;
			state.Set(key, cnt);
			return new Result<int>(cnt, state);
		};
	}
}