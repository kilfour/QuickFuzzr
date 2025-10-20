using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	public static Generator<T[]> ToArray<T>(this Generator<IEnumerable<T>> generator)
	{
		return s => new Result<T[]>([.. generator(s).Value], s);
	}
}