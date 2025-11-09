using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	/// <summary>
	/// Creates a generator that shuffles the sequence produced by the source generator.
	/// Use when the collection is itself generated (e.g., from <c>.Many(...)</c>) and you
	/// want an unbiased random ordering for each generation.
	/// </summary>
	public static FuzzrOf<IEnumerable<T>> Shuffle<T>(this FuzzrOf<IEnumerable<T>> source)
	{
		return state =>
		{
			var seqResult = source(state);
			var arr = seqResult.Value is T[] a ? (T[])a.Clone() : [.. seqResult.Value];

			for (int i = arr.Length - 1; i > 0; i--)
			{
				var draw = Fuzzr.Int(0, i + 1)(state);
				int j = draw.Value;

				(arr[i], arr[j]) = (arr[j], arr[i]);
			}

			return new Result<IEnumerable<T>>(arr, state);
		};
	}
}