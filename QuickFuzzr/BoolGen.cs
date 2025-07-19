using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzz
	{
		public static Generator<bool> Bool()
		{
			return s => new Result<bool>(s.Random.Next(0, 2) > 0, s);
		}
	}
}