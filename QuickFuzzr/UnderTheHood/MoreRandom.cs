namespace QuickFuzzr.UnderTheHood
{
	public class MoreRandom
	{
		private readonly Random random;

		public MoreRandom(int seed)
		{
			random = new Random(seed);
		}

		public int Next(int minimumValue, int maximumValue)
		{
			if (maximumValue <= minimumValue)
				return minimumValue;

			return random.Next(minimumValue, maximumValue);
		}

		public double NextDouble()
		{
			return random.NextDouble();
		}

		public long NextInt64(long min, long max)
		{
			return random.NextInt64(min, max);
		}
	}
}