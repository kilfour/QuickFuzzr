using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<char> Char() => Char('a', 'z');
	public static FuzzrOf<char> Char(char min, char max)
		=> MinMax.Check(min, max, state => new Result<char>((char)state.Random.Next(min, max), state));
}
