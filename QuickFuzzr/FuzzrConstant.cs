using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that always produces the same constant value regardless of generation state.
	/// Use for fixed values in composed Fuzzrs, default parameters, or when you need predictable data within random generation contexts.
	/// </summary>
	public static FuzzrOf<T> Constant<T>(T value)
	{
		return s => new Result<T>(value, s);
	}
}