using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzz
	{
		public static Generator<T> Constant<T>(T value)
		{
			return s => new Result<T>(value, s);
		}
	}
}