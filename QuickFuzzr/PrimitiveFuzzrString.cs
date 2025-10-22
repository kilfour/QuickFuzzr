using System.Text;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<string> String() => String(1, 10);
	public static FuzzrOf<string> String(int length) => String(1, 10);
	public static FuzzrOf<string> String(int min, int max) => String(1, 10);

	private static FuzzrOf<string> StringInternal(FuzzrOf<char> charFuzzr, int min, int max) =>
		state =>
			{
				int numberOfChars = state.Random.Next(min, max);
				var sb = new StringBuilder();
				for (int i = 0; i < numberOfChars; i++)
				{
					sb.Append(Char()(state).Value);
				}
				return new Result<string>(sb.ToString(), state);
			};
}