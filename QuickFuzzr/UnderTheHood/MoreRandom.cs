namespace QuickFuzzr.UnderTheHood;

public class MoreRandom(int seed)
{
	private readonly Random random = new(seed);

	public int Next(int minimumValue, int maximumValue)
	{
		if (maximumValue <= minimumValue)
			return minimumValue;

		return random.Next(minimumValue, maximumValue);
	}

	public void NextBytes(byte[] buffer) => random.NextBytes(buffer);

	public double NextDouble() => random.NextDouble();

	public long NextInt64(long min, long max) => random.NextInt64(min, max);
}