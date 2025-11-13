using System.Text;
using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a fuzzr that produces random strings of lowercase letters with length between 1 and 10 (inclusive) characters .
	/// Use for generating simple text data like names, identifiers, or any scenario requiring basic string content.
	/// </summary>
	public static FuzzrOf<string> String() => StringInternal(Char(), 1, 10);

	/// <summary>
	/// Creates a fuzzr that produces random strings of lowercase letters with exactly the specified length.
	/// Use when you need fixed-length strings for testing validation logic, fixed-width formats, or length-specific requirements.
	/// </summary>
	public static FuzzrOf<string> String(int length) => StringInternal(Char(), length, length);

	/// <summary>
	/// Creates a fuzzr that produces random strings of lowercase letters with length between the specified min and max (inclusive).
	/// Use for generating variable-length text data to test boundary conditions and length validation logic.
	/// </summary>
	public static FuzzrOf<string> String(int min, int max) => StringInternal(Char(), min, max);

	/// <summary>
	/// Creates a fuzzr that produces random strings using the specified character fuzzr with length between 1 and 10 (inclusive) characters.
	/// Use when you need custom character sets like digits, uppercase letters, or symbols in your generated strings.
	/// </summary>
	public static FuzzrOf<string> String(FuzzrOf<char> charFuzzr) => StringInternal(charFuzzr, 1, 10);

	/// <summary>
	/// Creates a fuzzr that produces random strings using the specified character fuzzr with exactly the specified length.
	/// Use for fixed-length strings with custom character composition like numeric codes, specialized identifiers, or formatted data.
	/// </summary>
	public static FuzzrOf<string> String(FuzzrOf<char> charFuzzr, int length) => StringInternal(charFuzzr, length, length);

	/// <summary>
	/// Creates a fuzzr that produces random strings using the specified character fuzzr with length between min and max (inclusive).
	/// Use for variable-length strings with controlled character sets to test complex text processing and validation logic.
	/// </summary>
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