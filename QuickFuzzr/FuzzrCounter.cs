using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<int> Counter()
	{
		var key = System.Guid.NewGuid();
		return state =>
		{
			var cnt = state.Get(key, 0) + 1;
			state.Set(key, cnt);
			return new Result<int>(cnt, state);
		};
	}
}