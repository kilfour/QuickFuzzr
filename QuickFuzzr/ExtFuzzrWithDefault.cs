using QuickFuzzr.UnderTheHood;
using QuickFuzzr.UnderTheHood.WhenThingsGoWrong;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a fuzzr that substitutes a default value when the source fuzzr
	/// fails due to an <see cref="OneOfEmptyOptionsException"/>.
	/// Use for safe fallback generation when working with potentially empty choice
	/// collections or dynamic data sources.
	/// </summary>
	public static FuzzrOf<T> WithDefault<T>(this FuzzrOf<T> fuzzr, T def = default!) =>
		state =>
			{
				try { return fuzzr(state); }
				catch (OneOfEmptyOptionsException) { return new Result<T>(def!, state); }
			};
}