using QuickFuzzr.Tests._Tools;
using QuickFuzzr.Tests._Tools.Models;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.B_Reference.P_Primitives.B_Other;

[DocFile]
[DocContent("Use `Fuzzr.String()`.")]
[DocColumn(PrimitiveFuzzrs.Columns.Description, "Creates random lowercase strings (default length 1-10).")]
public class Strings
{
	private static readonly HashSet<char> Valid = [.. "abcdefghijklmnopqrstuvwxyz"];

	[Fact]
	[DocContent("- The Default Fuzzr generates a string of length greater than or equal to 1 and less than or equal to 10.")]
	public void DefaultMinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.String(),
			("length >= 1", a => a?.Length >= 1), ("length <= 10", a => a?.Length <= 10));

	[Fact]
	[DocContent("- The overload `Fuzzr.String(int min, int max)` generates a string of length greater than or equal to `min` and less than or equal to `max`.")]
	public void MinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.String(5, 7),
			("length >= 5", a => a?.Length >= 5), ("length <= 7", a => a?.Length <= 7));

	[Fact]
	[DocContent("- Throws an `ArgumentException` when `min` > `max`.")]
	public void MinGreaterThanMax_Throws()
		=> Assert.Throws<ArgumentOutOfRangeException>(() => Fuzzr.String(5, 4).Generate());

	[Fact]
	[DocContent("- The overload `Fuzzr.String(int length)` generates a string of exactly `length` ... erm ... length.")]
	public void Length()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.String(5),
			("length == 5", a => a?.Length == 5));

	[Fact]
	[DocContent("- Throws an `ArgumentOutOfRangeException` when `length` < 0.")]
	public void LengthNegative_Throws()
		=> Assert.Throws<ArgumentOutOfRangeException>(() => Fuzzr.String(-1).Generate());

	[Fact]
	public void LengthZero()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.String(0),
			("is string.Empty", a => a == string.Empty));

	[Fact]
	public void MinMaxZero()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.String(0, 0),
			("is string.Empty", a => a == string.Empty));

	[Fact]
	public void Boundaries()
		=> CheckIf.GeneratedValuesShouldEventuallySatisfyAll(Fuzzr.String(1, 2),
			("length == 1", a => a.Length == 1), ("length == 2", a => a.Length == 2));

	[Fact]
	[DocContent("- The default Fuzzr always generates every char element of the string to be between lower case 'a' and lower case 'z'.")]
	public void DefaultFuzzrStringElementsAlwaysBetweenLowerCaseAAndLowerCaseZ()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.String(),
			("is letter", a => a.All(Valid.Contains)));

	[Fact]
	[DocContent("- A version exists for all methods mentioned above that takes a `FuzzrOf<char>` as parameter and then this one will be used to build up the resulting string.")]
	public void CustomCharFuzzr()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.String(Fuzzr.Char('a', 'a')),
			("is 'a'", a => a.All(b => b == 'a')));

	[Fact]
	public void CustomCharFuzzrLength()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.String(Fuzzr.Char('a', 'a'), 5),
			("is 'a'", a => a.All(b => b == 'a')));

	[Fact]
	public void CustomCharFuzzrMinMax()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.String(Fuzzr.Char('a', 'a'), 1, 3),
			("is 'a'", a => a.All(b => b == 'a')));

	[Fact]
	public void Nullable()
		=> CheckIf.GeneratesNullAndNotNull(Fuzzr.String().NullableRef());

	[Fact]
	public void Property()
		=> CheckIf.GeneratedValuesShouldAllSatisfy(Fuzzr.One<StringBag>(),
			("value.Length >= 1", a => a.Value.Length >= 1));

	[Fact]
	public void NullableProperty()
		=> CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<StringBag>().Select(a => a.NullableValue));
}