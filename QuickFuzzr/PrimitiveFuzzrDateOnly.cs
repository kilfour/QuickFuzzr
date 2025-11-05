using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a generator that produces random DateOnly values between January 1, 1970 and December 31, 2020 (inclusive).
	/// Use for generating realistic date values within a common historical range for testing temporal logic.
	/// </summary>
	public static FuzzrOf<DateOnly> DateOnly()
		=> DateOnly(new DateOnly(1970, 1, 1), new DateOnly(2020, 12, 31));

	/// <summary>
	/// Creates a generator that produces random DateOnly values within the specified inclusive date range.
	/// Use when you need date values constrained to specific time periods for business logic testing.
	/// </summary>
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