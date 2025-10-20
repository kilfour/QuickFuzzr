using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class ExtFuzzr
{
	public static Generator<T> Where<T>(this Generator<T> generator, Func<T, bool> predicate)
	{
		return
			s =>
				{
					for (int i = 0; i < 50; i++)
					{
						var candidate = generator(s);
						if (predicate(candidate.Value))
							return candidate;
					}
					throw new HeyITriedFiftyTimesButCouldNotGetADifferentValue();
				};
	}
}