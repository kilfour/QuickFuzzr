using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that returns the provided values in order, repeating from the
	/// beginning after the final value.
	/// </summary>
	public static FuzzrOf<T> Sequence<T>(params T[] values)
	{
		ArgumentNullException.ThrowIfNull(values);
		if (values.Length == 0)
			throw new ArgumentException("The sequence must contain at least one value.", nameof(values));

		var snapshot = (T[])values.Clone();
		var key = new object();
		return state =>
		{
			var index = state.Get(key, 0);
			state.Set(key, (index + 1) % snapshot.Length);
			return new Result<T>(snapshot[index], state);
		};
	}
}
