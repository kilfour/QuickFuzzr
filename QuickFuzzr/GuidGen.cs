using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzz
	{
		public static Generator<Guid> Guid()
		{
			return s => new Result<Guid>(System.Guid.NewGuid(), s);
		}
	}
}