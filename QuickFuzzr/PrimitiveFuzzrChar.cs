using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static Generator<char> Char()
	{
		const int lowerCaseLetterACode = 97;
		const int lowerCaseLetterZCode = 122;
		return s => new Result<char>((char)s.Random.Next(lowerCaseLetterACode, lowerCaseLetterZCode + 1), s);
	}
}