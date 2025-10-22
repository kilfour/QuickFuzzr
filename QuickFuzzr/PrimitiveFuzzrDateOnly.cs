using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	public static FuzzrOf<DateOnly> DateOnly()
		=> DateOnly(new DateOnly(1970, 1, 1), new DateOnly(2020, 12, 31));

	public static FuzzrOf<DateOnly> DateOnly(DateOnly min, DateOnly max)
		=> MinMax.Check(min, max, DateOnlyInternal(min, max));

	private static FuzzrOf<DateOnly> DateOnlyInternal(DateOnly min, DateOnly max) =>
		state =>
			{
				var minN = min.DayNumber;
				var maxN = max.DayNumber;
				var span = (long)maxN - minN;
				var offset = state.Random.Next(0, checked((int)span));
				var value = System.DateOnly.FromDayNumber(minN + offset);
				return new Result<DateOnly>(value, state);
			};
}