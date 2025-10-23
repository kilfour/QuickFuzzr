using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<Guid> Guid()
	{
		return s =>
		{
			var bytes = new byte[16];
			s.Random.NextBytes(bytes);
			var value = new Guid(bytes);
			return new Result<Guid>(value, s);
		};
	}
}
