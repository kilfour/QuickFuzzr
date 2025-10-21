using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<T> Constant<T>(T value)
	{
		return s => new Result<T>(value, s);
	}
}