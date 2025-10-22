using System.Text;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<string> String() => StringInternal(Char(), 1, 10);
	public static FuzzrOf<string> String(int length) => StringInternal(Char(), length, length);
	public static FuzzrOf<string> String(int min, int max) => StringInternal(Char(), min, max);
	public static FuzzrOf<string> String(FuzzrOf<char> charFuzzr) => StringInternal(charFuzzr, 1, 10);
	public static FuzzrOf<string> String(FuzzrOf<char> charFuzzr, int length) => StringInternal(charFuzzr, length, length);
	public static FuzzrOf<string> String(FuzzrOf<char> charFuzzr, int min, int max) => StringInternal(charFuzzr, min, max);
	private static FuzzrOf<string> StringInternal(FuzzrOf<char> charFuzzr, int min, int max) =>
		state =>
			{
				ArgumentOutOfRangeException.ThrowIfNegative(min);
				MinMax.Check(min, max);
				int numberOfChars = state.Random.Next(min, max + 1);
				var sb = new StringBuilder();
				for (int i = 0; i < numberOfChars; i++)
				{
					sb.Append(charFuzzr(state).Value);
				}
				return new Result<string>(sb.ToString(), state);
			};
}