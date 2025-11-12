using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a fuzzr that boxes values from the source fuzzr into object references.
	/// Use when you need type-erased values, heterogeneous collections, or when working with reflection-based APIs that require object parameters.
	/// </summary>
	public static FuzzrOf<object> AsObject<T>(this FuzzrOf<T> fuzzr)
	{
		return s => new Result<object>(fuzzr(s).Value!, s);
	}
}