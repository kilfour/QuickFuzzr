using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzzr
	{
		public static Generator<T> Unique<T>(this Generator<T> generator, Func<object> key)
		{
			return
				s =>
					{
						var allreadyGenerated = s.Get(key(), new List<T>());
						for (int i = 0; i < 50; i++)
						{
							var result = generator(s);
							if (!allreadyGenerated.Contains(result.Value))
							{
								allreadyGenerated.Add(result.Value);
								return result;
							}
						}
						throw new HeyITriedFiftyTimesButCouldNotGetADifferentValue($"(key: {key()})");
					};
		}

		public static Generator<T> Unique<T>(this Generator<T> generator, object key)
		{
			return
				s =>
					{
						var allreadyGenerated = s.Get(key, new List<T>());
						for (int i = 0; i < 50; i++)
						{
							var result = generator(s);
							if (!allreadyGenerated.Contains(result.Value))
							{
								allreadyGenerated.Add(result.Value);
								return result;
							}
						}
						throw new HeyITriedFiftyTimesButCouldNotGetADifferentValue($"(key: {key})");
					};
		}
	}
}