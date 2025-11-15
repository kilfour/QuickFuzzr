using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces random lowercase alphabetic characters ('a'-'z').
	/// Use for generating simple text fragments, identifiers, or single-character fields.
	/// </summary>
	public static FuzzrOf<char> Char() => Char('a', 'z');

	/// <summary>
	/// Creates a Fuzzr that produces random characters within the inclusive range [min, max].
	/// Use for generating characters from specific ranges like uppercase letters, digits, or custom character sets.
	/// </summary>
	public static FuzzrOf<char> Char(char min, char max)
		=> MinMax.Check(min, max, state => new Result<char>((char)state.Random.Next(min, max + 1), state));
}
