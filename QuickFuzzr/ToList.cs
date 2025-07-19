using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzz
	{
		public static Generator<List<T>> ToList<T>(this Generator<IEnumerable<T>> generator)
		{
			return s => new Result<List<T>>(generator(s).Value.ToList(), s);
		}
	}
}