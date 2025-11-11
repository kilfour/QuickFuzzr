using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a generator that produces a random permutation of the provided items.
	/// Use for randomized ordering, sampling without replacement, and unbiased permutations.
	/// </summary>
	public static FuzzrOf<IEnumerable<T>> Shuffle<T>(params T[] values) =>
		Shuffle((IEnumerable<T>)values);

	/// <summary>
	/// Creates a generator that produces a random permutation of the provided collection.
	/// Use for randomized ordering, sampling without replacement, and unbiased permutations.
	/// </summary>
	public static FuzzrOf<IEnumerable<T>> Shuffle<T>(IEnumerable<T> values)
	{
		var snapshot = values.ToArray();
		return state =>
		{
			var arr = (T[])snapshot.Clone();
			for (int i = arr.Length - 1; i > 0; i--)
			{
				var draw = Int(0, i + 1)(state);
				int j = draw.Value;
				(arr[i], arr[j]) = (arr[j], arr[i]);
			}
			return new Result<IEnumerable<T>>(arr, state);
		};
	}
}