using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<Guid> Guid()
	{
		return s => new Result<Guid>(System.Guid.NewGuid(), s);
	}
}