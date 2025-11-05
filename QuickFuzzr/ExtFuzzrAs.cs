using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a generator that boxes values from the source generator into object references.
	/// Use when you need type-erased values, heterogeneous collections, or when working with reflection-based APIs that require object parameters.
	/// </summary>
	public static FuzzrOf<object> AsObject<T>(this FuzzrOf<T> generator)
	{
		return s => new Result<object>(generator(s).Value!, s);
	}
}