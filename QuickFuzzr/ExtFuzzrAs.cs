using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	public static FuzzrOf<object> AsObject<T>(this FuzzrOf<T> generator)
	{
		return s => new Result<object>(generator(s).Value!, s);
	}

	public static FuzzrOf<string> AsString<T>(this FuzzrOf<T> generator)
	{
		return s => new Result<string>(generator(s).Value!.ToString()!, s);
	}
}