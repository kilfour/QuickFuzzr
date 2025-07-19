using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class MGen
	{
		public static T Generate<T>(this Generator<T> generator)
		{
			return generator(new State()).Value;
		}
	}
}