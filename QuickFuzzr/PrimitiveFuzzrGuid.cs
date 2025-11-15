using QuickFuzzr.UnderTheHood;

namespace QuickFuzzr;

public static partial class Fuzzr
{
	/// <summary>
	/// Creates a Fuzzr that produces random GUID values with full 128-bit randomness.
	/// Use for generating unique identifiers, database keys, or any scenario requiring universally unique values for testing.
	/// This Fuzzr never generates Guid.Empty and is deterministic when using a seed.
	/// </summary>
	public static FuzzrOf<Guid> Guid()
	{
		return s =>
		{
			var bytes = new byte[16];
			s.Random.NextBytes(bytes);
			var value = new Guid(bytes);
			return new Result<Guid>(value, s);
		};
	}
}
