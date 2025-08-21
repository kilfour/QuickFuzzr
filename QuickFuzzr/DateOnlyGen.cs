using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr
{
	public static partial class Fuzz
	{
		public static Generator<DateOnly> DateOnly()
		=> DateOnly(new DateOnly(1970, 1, 1), new DateOnly(2020, 12, 31));

		public static Generator<DateOnly> DateOnly(DateOnly min, DateOnly max)
		{
			if (max < min) throw new ArgumentOutOfRangeException(nameof(max), "max < min");

			var minN = min.DayNumber;
			var maxN = max.DayNumber;
			var span = (long)maxN - minN;

			return s =>
			{
				var offset = s.Random.Next(0, checked((int)span));
				var value = System.DateOnly.FromDayNumber(minN + offset);
				return new Result<DateOnly>(value, s);
			};
		}
	}
}