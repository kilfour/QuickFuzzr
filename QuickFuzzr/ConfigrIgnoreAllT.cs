using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Configr<T>
{
	/// <summary>
	/// Creates a Fuzzr that configures all properties of type T to be ignored during automatic generation.
	/// Use when you want to completely disable auto-generation for a specific type and handle all property population manually.
	/// </summary>
	public static FuzzrOf<Intent> IgnoreAll() =>
		state =>
		{
			state.StuffToIgnoreAll.Add(typeof(T));
			return Result.Unit(state);
		};
}