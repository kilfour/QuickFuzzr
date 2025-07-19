using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzz
	{
		public static Generator<object> Primitive(Type type)
		{
			return s => s.PrimitiveGenerators[type](s);
		}
	}
}